using Game.Grid;
using Game.Input;
using Game.Unit;
using GetraenkeBub;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game.GameModel;

namespace Game
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private GridPresenter[] levels;
        private List<UnitPresenter> units;
        private AUserInput lastUserInput;

        private IEnumerator gameFlow;

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
            units = currentLevel.FindGetUnits().OrderByDescending(p => p.GetInitiative()).ToList();

            for(int i = 0; i < currentLevel.GetRoundCount(); i++)
            {
                units.ForEach(p => p.ApplyOverallRoundStart());
                foreach(UnitPresenter unit in units)
                {
                    bool unitIsFinished = false;
                    while(!unitIsFinished)
                    {
                        List<AAbility> abilities = unit.GetAbilityOptions();
                        List<(AAbility, AbilityUsability)> tuples = abilities.Select(a => (a, 
                            a.actionPointCost > unit.GetAbilityPoints()? AbilityUsability.NotEnoughEnergy:
                            (!a.IsTargetConditionSatisfied() ? AbilityUsability.TargetNotAvailable:
                            AbilityUsability.Castable))
                        ).ToList();
                        UIStateManager.Instance.SetAbilities(tuples);

                        HashSet<Vector2Int> movementOptions = new HashSet<Vector2Int>();
                        if (unit.GetCurrentMovementPoints() > 0)
                        {
                            movementOptions = unit.GetMovementOptions();
                            foreach (Vector2Int item in movementOptions)
                            {
                                GridPresenter.Instance.GetContent(item).SetHighlightOption(AGridContent.HighlightOption.Movement);
                            }                           
                        }

                        yield return null;

                        bool waitForAnimation = false;
                        switch (lastUserInput)
                        {
                            case MovementInput movementInput:
                                if (movementOptions.Contains(movementInput.position))
                                {
                                    unit.ApplyCurrentMovementPointChange(-1);
                                    LeanTween.delayedCall(0.01f, () => actionInput.ability.Cast(WaitFinishedHandler)); //TODO movement
                                    waitForAnimation = true;
                                }
                                break;
                            case AbilityInput actionInput:
                                LeanTween.delayedCall(0.01f, () => actionInput.ability.Cast(WaitFinishedHandler));
                                unitIsFinished = true;
                                waitForAnimation = true;
                                break;
                        }

                        while (waitForAnimation)
                        {
                            yield return null;
                            if(lastUserInput is FinishAnimationInput)
                            {
                                waitForAnimation = false;
                            }
                        }
                    }
                }
            }
            
            yield return null;
        }
        #region PlayLevel Helper Functions
        private void LoadLevel(int index)
        {
            for(int i=0; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(i == index);
            }
        }
        #endregion
    }
}