using System.Collections.Generic;
using UnityEngine;

namespace GetraenkeBub
{
    public class UIStateManager : MonoBehaviour
    {

        public static UIStateManager Instance;
        private UIState currentUIState;
        [SerializeField] private EUIState startUIState;

        [SerializeField] private List<UIState> UIStates;
        private void Awake()
        {
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
            IUIState = UIManager.GetComponent<IUIState>();
            IUIState.Init();
       }

        public void OnBeforeEnter()
        {
            IUIState.OnBeforeEnter();
        }
        public void OnEnter()
        {
            IUIState.OnEnter();
        }

        public void OnLeave()
        {
            IUIState.OnLeave();
        }

    }
}
