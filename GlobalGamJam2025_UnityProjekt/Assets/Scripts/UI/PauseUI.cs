using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class PauseUI : MonoBehaviour , IUIState
    {

        [SerializeField] private Button backToMenuButton;
        [SerializeField] private Button backButton;

        public void Init()
        {
            backButton.onClick.AddListener(BackToPlayerUI);
            backToMenuButton.onClick.AddListener(() => UIStateManager.Instance.GetBackToMenu());
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


        public void BackToPlayerUI()
        {
            UIStateManager.Instance.ChangeUIState(EUIState.PlayerUI);
        }

        public void ChangeToEnglish()
        {
            TODO.LOCA = false;
        }

        public void ChangeToDeutsch()
        {
            TODO.LOCA = false;
        }
    }
}
