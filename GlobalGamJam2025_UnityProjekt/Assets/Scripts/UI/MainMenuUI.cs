using System;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class MainMenuUI : MonoBehaviour ,IUIState
    {

        [SerializeField] private Button closeButton;
        [SerializeField] private Button howToPlayButton;
        [SerializeField] private Button startLevel1Button;
        [SerializeField] private Button startLevel2Button;
        [SerializeField] private Button startLevel3Button;


        public void Init()
        {
            closeButton.onClick.AddListener(() => UIStateManager.Instance.CloseGame());
            startLevel1Button.onClick.AddListener(StartLevel1);
            startLevel2Button.onClick.AddListener(StartLevel2);
            startLevel3Button.onClick.AddListener(StartLevel3);
            howToPlayButton.onClick.AddListener(ShowHowToPlay);
        }

        private void ShowHowToPlay()
        {
            UIStateManager.Instance.ChangeUIState(EUIState.HowToPlay);
        }

        private void StartLevel3()
        {
            Debug.Log("StartLevel1");
        }

        private void StartLevel2()
        {
            Debug.Log("StartLevel2");
        }

        private void StartLevel1()
        {
            Debug.Log("StartLevel3");
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
