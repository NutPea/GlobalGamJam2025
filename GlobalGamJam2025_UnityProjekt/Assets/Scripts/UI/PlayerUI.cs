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

        [SerializeField] private List<CharacterStateUIPresenter> playerCharacterStateUIPresenter;
        [SerializeField] private List<CharacterStateUIPresenter> enemyCharacterStateUIPresenter;
        int currentPlayerStateIndex = 0;
        int currentEnemyStateIndex = 0;
        [Header("Buttons")]
        [SerializeField] private Button pauseButton;

        private List<UnitPresenter> allUnitPresenter;


        public void Init()
        {
            pauseButton.onClick.AddListener(ChangeToPause);
        }

        public void OnBeforeEnter()
        {
            currentPlayerStateIndex = 0;
            currentEnemyStateIndex = 0;
            playerCharacterStateUIPresenter.ForEach(n => n.gameObject.SetActive(false));
            enemyCharacterStateUIPresenter.ForEach(n => n.gameObject.SetActive(false));

            Debug.Log(GridPresenter.Instance);
            allUnitPresenter = GridPresenter.Instance.GetAll<UnitPresenter>();
            foreach (UnitPresenter presenter in GridPresenter.Instance.GetAll<UnitPresenter>())
            {
                if(presenter != null)
                {
                    allUnitPresenter.Add(presenter);
                    if(presenter.GetFaction() == UIStateManager.Instance.playerFaction)
                    {
                        playerCharacterStateUIPresenter[currentPlayerStateIndex].Setup(presenter, UIStateManager.Instance.currentUnitPresenter);
                        currentPlayerStateIndex++;
                    }
                    else
                    {
                        enemyCharacterStateUIPresenter[currentPlayerStateIndex].Setup(presenter, UIStateManager.Instance.currentUnitPresenter);
                        currentEnemyStateIndex ++;
                    }
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
