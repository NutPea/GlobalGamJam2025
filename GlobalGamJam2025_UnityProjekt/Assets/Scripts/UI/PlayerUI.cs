using Game;
using Game.Grid;
using Game.Unit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class PlayerUI : MonoBehaviour, IUIState
    {

        [Header("Buttons")]
        [SerializeField] private Button pauseButton;

        private List<UnitPresenter> allUnitPresenter;


        public void Init()
        {
            pauseButton.onClick.AddListener(ChangeToPause);
        }

        public void OnBeforeEnter()
        {
           foreach(UnitPresenter presenter in GridPresenter.Instance.GetAll<UnitPresenter>())
           {
                if(presenter != null)
                {
                    allUnitPresenter.Add(presenter);
                }
           }


        }

        public void OnEnter()
        {
            
        }

        public void OnLeave()
        {
         
        }

        private void ChangeToPause()
        {
            Debug.Log("Miep");
            UIStateManager.Instance.ChangeUIState(EUIState.PauseUI);
        }
    }
}
