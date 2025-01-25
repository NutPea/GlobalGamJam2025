using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Game.GameModel;

namespace GetraenkeBub
{
    public class UIStateManager : MonoBehaviour
    {

        public static UIStateManager Instance;

        public event Action<AAbility> OnAbilityCasted; //TODO INVOKE!

        private UIState currentUIState;
        [SerializeField] private EUIState startUIState;

        [SerializeField] private List<UIState> UIStates;
        private void Awake()
        {
            Instance = this;
            UIStates.ForEach(n => n.OnInit());
            ChangeUIState(startUIState);
        }

        public void ChangeUIState(EUIState toChangeUiState)
        {
            if(currentUIState != null)
            {
                currentUIState.OnLeave();
            }
            currentUIState = GetUISTate(toChangeUiState);
            currentUIState.OnBeforeEnter();
            currentUIState.OnEnter();

        }

        public void SetAbilities(List<(AAbility, AbilityUsability)> abilities)
        {
            //TODO PEANUT
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

        public void HandleAbility(Action done)
        {
            done.Invoke();
        }

    }


    [System.Serializable]
    public class UIState{

        [SerializeField] private GameObject UIManager;
        [SerializeField]private EUIState uiState;
        private IUIState IUIState;

        public bool CheckIfUIState(EUIState toCheckUIState)
        {
            return uiState == toCheckUIState;
        }

       public void OnInit()
       {
            try
            {
                IUIState = UIManager.GetComponent<IUIState>();
                IUIState.Init();
                UIManager.gameObject.SetActive(false);
            }
            catch(Exception e)
            {
                Debug.Log(UIManager.name + e);
            }
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
