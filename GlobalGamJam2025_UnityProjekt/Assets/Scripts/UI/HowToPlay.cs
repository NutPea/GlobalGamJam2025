using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class HowToPlay : MonoBehaviour, IUIState
    {

        [SerializeField] private List<Sprite> tutorialSprites;
        [SerializeField] private List<string> tutorialTextStrings;

        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] private Image tutorialImage;

        [SerializeField] private Button nextButton;
        int tutorialIndex;

        public void Init()
        {
            nextButton.onClick.AddListener(ShowNextTutorialInput);
        }

        private void ShowNextTutorialInput()
        {
            tutorialIndex++;
            ShowNextTutorial();
        }

        private void ShowNextTutorial()
        {
            if(tutorialIndex > tutorialSprites.Count - 1)
            {
                UIStateManager.Instance.ChangeUIState(EUIState.MainMenu);
            }
            else
            {
                tutorialText.text = tutorialTextStrings[tutorialIndex];
                tutorialImage.sprite = tutorialSprites[tutorialIndex];
            }
        }



        public void OnBeforeEnter()
        {
            tutorialIndex = 0;
            ShowNextTutorial();
        }

        public void OnEnter()
        {
            
        }

        public void OnLeave()
        {
           
        }
    }
}
