using AI;
using Game.Community;
using Game.Grid;
using Game.Input;
using Game.Spawner;
using Game.Unit;
using GetraenkeBub;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game.GameModel;

namespace Game
{
    public class GamePresenter : MonoBehaviour
    {
        public event Action<ITarget, ITarget> OnTargetChanged; //UI updaten
        public event Action<int, int> OnRoundCounterChanged;
        public event Action OnGameOver;
        public event Action OnLastRoundOver;

        public event Action<int, int> OnPointsChanged;

        public static GamePresenter Instance;

        [SerializeField] private GridPresenter[] levels;
        private List<UnitPresenter> units;
        private List<CommunityPresenter> communities;
        private List<SpawnerPresenter> spawners;

        private AUserInput lastUserInput;
        private GameModel model;

        private IEnumerator gameFlow;

        private void Awake()
        {
            model = GetComponent<GameModel>();

            model.Points.OnChange += OnPointChangeHandler;
            Instance = this;
        }
        private void Start()
        {
            gameFlow = PlayLevel(0);
            gameFlow.MoveNext();

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UIStateManager.Instance.OnAbilityCasted += AbilityCastedHandler;
            GridPresenter.Instance.OnGridPressed += GridClickedHandler;

        }
        #region SubscribeEvents Handler Functions
        private void AbilityCastedHandler(AAbility ability)
        {
            lastUserInput = new AbilityInput(ability);
            gameFlow?.MoveNext();
        }
        private void GridClickedHandler(Vector2Int position)
        {
            lastUserInput = new MovementInput(position);
            gameFlow?.MoveNext();
        }
        private void WaitFinishedHandler()
        {
            lastUserInput = new FinishAnimationInput();
            gameFlow?.MoveNext();
        }
        #endregion

        private IEnumerator PlayLevel(int index)
        {
            LoadLevel(index);
            GridPresenter currentLevel = GridPresenter.Instance;

            units = UpdateContent<UnitPresenter>();
            spawners =  UpdateContent<SpawnerPresenter>();
            communities = UpdateContent<CommunityPresenter>();

            ITarget lastTarget = null;
            for (int i = 0; i < currentLevel.GetRoundCount(); i++)
            {
                //round start logic
                OnRoundCounterChanged?.Invoke(Mathf.Max(0, i - 1), i);
                units.ForEach(p => p.ApplyOverallRoundStart());

                foreach (var item in AIDirector.Instances)
                {
                    item.Value.RoundStart();
                }

                //update spawners
                foreach (SpawnerPresenter spawner in spawners)
                {
                    LeanTween.delayedCall(spawner.GetTurnFocusDuration(), () => spawner.UpdateSpawner());
                    yield return WaitForFinishTurn();
                    units = UpdateContent<UnitPresenter>();
                }

                //do unit turns
                foreach (UnitPresenter unit in units)
                {
                    if(unit.gameObject == null) continue;
                    unit.ApplyIndividualRoundStart();
                    OnTargetChanged?.Invoke(lastTarget, unit);

                    bool unitIsFinished = false;
                    while(!unitIsFinished)
                    {
                        UpdateAbilities(unit);
                        HashSet<Vector2Int> movementOptions = UpdateMovement(unit);
                        yield return null;

                        If_AI_TakeTurn(unit);

                        bool waitForAnimation = false;
                        switch (lastUserInput)
                        {
                            case MovementInput movementInput:
                                HandleMovement(movementOptions, movementInput, unit);
                                break;
                            case AbilityInput abilityInput:
                                HandleAbility(unit, abilityInput, ref unitIsFinished, ref waitForAnimation);
                                break;
                        }

                        while (waitForAnimation)
                        {
                            yield return null;
                            CheckInterruptWait(ref waitForAnimation);
                        }
                    }

                    if(!units.Any(u => u.GetFaction() == UnitModel.Faction.Vegans))
                    {
                        OnGameOver?.Invoke();
                        gameFlow = null;
                        yield return null;
                    }                    
                }

                foreach (CommunityPresenter community in communities)
                {
                    LeanTween.delayedCall(community.GetTurnFocusDuration(), () => community.UpdateCommunity());
                    yield return WaitForFinishTurn();
                }
            }
            OnLastRoundOver?.Invoke();

            yield return null;
        }
        #region PlayLevel Helper Functions
        private List<T> UpdateContent<T>() where T:ITarget
        {
            return GridPresenter.Instance.GetAll<T>().OrderByDescending(p => p.GetInitiative()).ToList();
        }
        private void LoadLevel(int index)
        {
            for(int i=0; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(i == index);
            }
        }
        private void UpdateAbilities(UnitPresenter unit)
        {
            List<AAbility> abilities = unit.GetAbilityOptions();
            List<(AAbility, AbilityUsability)> tuples = abilities.Select(a => (a,
                a.actionPointCost > unit.GetAbilityPoints() ? AbilityUsability.NotEnoughEnergy :
                (!a.IsTargetConditionSatisfied() ? AbilityUsability.TargetNotAvailable :
                AbilityUsability.Castable))
            ).ToList();
            UIStateManager.Instance.SetAbilities(tuples);
        }
        private HashSet<Vector2Int> UpdateMovement(UnitPresenter unit)
        {
            HashSet<Vector2Int> movementOptions = new();
            if (unit.GetCurrentMovementPoints() > 0)
            {
                movementOptions = unit.GetMovementOptions();
                foreach (Vector2Int item in movementOptions)
                {
                    GridPresenter.Instance.GetContent(item).SetHighlightOption(AGridContent.HighlightOption.Movement);
                }
            }
            return movementOptions;
        }
        private void HandleMovement(HashSet<Vector2Int> movementOptions, MovementInput movementInput, UnitPresenter unit)
        {
            if (movementOptions.Contains(movementInput.position))
            {
                unit.ApplyCurrentMovementPointChange(-1);
                GridPresenter.Instance.SwapCells(unit.GetPosition(), movementInput.position);
                GridPresenter.Instance.DisableAllGridHighlights();
                unit.SetPosition(movementInput.position);
            }
        }
        private void HandleAbility(UnitPresenter unit, AbilityInput abilityInput, ref bool unitIsFinished, ref bool waitForAnimation)
        {
            LeanTween.delayedCall(0.05f, () => abilityInput.ability.Cast(WaitFinishedHandler));
            unit.ApplyCurrentActionPointChange(-abilityInput.ability.actionPointCost);

            unitIsFinished = true;
            waitForAnimation = true;
            unit.ApplyIndividualRoundFinished();
        }
        private void CheckInterruptWait(ref bool waitForAnimation)
        {
            if (lastUserInput is FinishAnimationInput)
            {
                waitForAnimation = false;
            }
        }
        private void If_AI_TakeTurn(UnitPresenter unit)
        {
            if(unit.GetFaction() == UnitModel.Faction.Vegans)
            {
                return;
            }
            unit.GetComponent<AIAgent>().DoNextAction();
        }
        private IEnumerator WaitForFinishTurn()
        {
            bool turnFinished = false;
            while (!turnFinished)
            {
                yield return null;

                if(lastUserInput is FinishAnimationInput)
                {
                    turnFinished = true;
                }
            }
        }
        #endregion

        public List<UnitPresenter> GetUnits()
        {
            return units;
        }

        public void ChangePoints(int value)
        {
            model.Points.Value += value;
        }
        private void OnPointChangeHandler(int oldValue, int newValue)
        {
            OnPointsChanged?.Invoke(oldValue, newValue);
        }
    }
}