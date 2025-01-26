using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class GameOverUI : MonoBehaviour ,IUIState
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button closeButton;

        public void Init()
        {
            restartButton.onClick.AddListener(() => UIStateManager.Instance.GetBackToMenu());
            closeButton.onClick.AddListener(() => UIStateManager.Instance.CloseGame());
        }

        public void OnBeforeEnter()
        {
     
        }

        public void OnEnter()
        {
            
        }

        public void OnLeave()
        {
           
        }


    }

}

