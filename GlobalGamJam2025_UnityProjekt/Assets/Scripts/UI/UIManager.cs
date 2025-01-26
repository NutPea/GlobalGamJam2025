using Game;
using Game.Community;
using Game.Spawner;
using Game.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Game.GameModel;
using static Game.Unit.UnitModel;

namespace GetraenkeBub
{
    public class UIStateManager : MonoBehaviour
    {

        public static UIStateManager Instance;

        public event Action<AAbility> OnAbilityCasted; //TODO INVOKE!
        public event Action<AAbility> OnAbilityHighlight; //TODO INVOKE!
        public event Action OnAbilityStop; //TODO INVOKE!

        private UIState currentUIState;
        private UIState lastUIState;
        [SerializeField] private EUIState startUIState;
        [SerializeField] private List<AbilityButtonView> abilityButtonViews;
        [SerializeField] private CharacterAttackUI characterAttackUI;

        [SerializeField] private List<UIState> UIStates;
        public int AmountOfPoints = 0;
        public int CurrentRound = 0;
        public UnitPresenter currentUnitPresenter;
        public Faction playerFaction = Faction.Vegans;

        private void Awake()
        {
            Instance = this;

          

        }

        private void Start()
        {
            GamePresenter.Instance.OnGameOver += OnHandleGameOver;
            GamePresenter.Instance.OnLastRoundOver += OnHandleLastRound;
            GamePresenter.Instance.OnPointsChanged += OnPointsHandle;
            GamePresenter.Instance.OnRoundCounterChanged += OnHandleRoundCounterChange;
            GamePresenter.Instance.OnTargetChanged += HandleTargetChange;
            UIStates.ForEach(n => n.OnInit());
            ChangeUIState(startUIState);

        }

        private void OnPointsHandle(int arg1, int arg2)
        {
            AmountOfPoints = arg2;
        }

        private void HandleTargetChange(ITarget old, ITarget newTarget)
        {

            Transform foundTransform = null;
            switch(newTarget)
            {
                case UnitPresenter unitPresenter: foundTransform = unitPresenter.transform; break;
                case CommunityPresenter communityPresenter: foundTransform = communityPresenter.transform; break;
                case SpawnerPresenter spawnerPresenter: foundTransform = spawnerPresenter.transform; break;
                default: Debug.LogError("DAS sollte nicht passieren!"); break;
            }
            CameraManager.Instance.SetFollow(foundTransform);



            if(newTarget is UnitPresenter presenter)
            {
                Faction faction =  presenter.GetFaction();
                currentUnitPresenter = presenter;
                if (faction != playerFaction)
                {
                    ChangeUIState(EUIState.EnemyUI);
                }
                else
                {
                    ChangeUIState(EUIState.PlayerUI);
                }
            }
            else
            {
                ChangeUIState(EUIState.EnemyUI);
            }
        }

        private void OnHandleRoundCounterChange(int old, int newAmount)
        {
            CurrentRound = newAmount;

            ChangeUIState(EUIState.RoundChangeUI);
        }

        private void OnHandleLastRound()
        {
            ChangeUIState(EUIState.GameEnd);
        }

        private void OnHandleGameOver()
        {
            ChangeUIState(EUIState.GameOver);
        }

        public void InvokeOnAbilityHighlight(AAbility ability)
        {
            OnAbilityHighlight?.Invoke(ability);
        }


        public void InvokeOnAbilityCasted(AAbility ability)
        {
            OnAbilityCasted?.Invoke(ability);
        }

        public void InvokeOnAbilityStop()
        {
            OnAbilityStop?.Invoke();
        }

        public void ChangeUIState(EUIState toChangeUiState)
        {
            if(currentUIState != null)
            {
                currentUIState.OnLeave();
                lastUIState = currentUIState;
            }
            currentUIState = GetUISTate(toChangeUiState);
            currentUIState.OnBeforeEnter();
            currentUIState.OnEnter();

        }

        public void ReturnToLastUiState()
        {
            ChangeUIState(lastUIState.uIState);
        }

        public void SetAbilities(List<(AAbility, AbilityUsability)> abilities)
        {
            abilityButtonViews.ForEach(n => n.gameObject.SetActive(false));
            for(int i = 0; i< abilities.Count; i++)
            {
                (AAbility, AbilityUsability) abilityTopple = abilities[i];
                abilityButtonViews[i].gameObject.SetActive(true);
                abilityButtonViews[i].SetAbility(abilityTopple.Item1, abilityTopple.Item2);
            }
        }

        private UIState GetUISTate(EUIState toCheckUIState)
        {
            foreach(UIState uIState in UIStates)
            {
                if (uIState.CheckIfUIState(toCheckUIState))
                {
                    return uIState;
                }
            }
            return null;
        }

        public void HandleAbility(Action done, AAbility ability, GameObject caster, List<GameObject> targets, bool communitySuccess=false)
        {
            if(targets.Count == 0)
            {
                done?.Invoke();
            }
            else if(targets.Count == 1 && targets.Contains(caster))
            {
                done?.Invoke();
            }
            else
            {
                characterAttackUI.WasCommunityWasSuccsesfull = communitySuccess;
                characterAttackUI.HandleAbility(() => done.Invoke(),ability, caster, targets);
            }
        }

        public void GetBackToMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void CloseGame()
        {
            Application.Quit();
        }

    }


    [System.Serializable]
    public class UIState{

        [SerializeField] private GameObject UIManager;
        [SerializeField]private EUIState uiState;
        public EUIState uIState => uiState;
        private IUIState IUIState;

        public bool CheckIfUIState(EUIState toCheckUIState)
        {
            return uiState == toCheckUIState;
        }

       public void OnInit()
       {
            IUIState = UIManager.GetComponent<IUIState>();
            IUIState.Init();
            UIManager.gameObject.SetActive(false);

       }

        public void OnBeforeEnter()
        {
            UIManager.gameObject.SetActive(true);
            IUIState.OnBeforeEnter();
        }
        public void OnEnter()
        {
            IUIState.OnEnter();
        }

        public void OnLeave()
        {
            IUIState.OnLeave();
            UIManager.gameObject.SetActive(false);
        }


    }
}
