using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class PlayerUI : MonoBehaviour, IUIState
    {

        [Header("Buttons")]
        [SerializeField] private Button pauseButton;


        public void Init()
        {
            pauseButton.onClick.AddListener(ChangeToPause);
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

        private void ChangeToPause()
        {
            Debug.Log("Miep");
            UIStateManager.Instance.ChangeUIState(EUIState.PauseUI);
        }
    }
}
