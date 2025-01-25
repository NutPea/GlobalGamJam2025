using TMPro;
using UnityEngine;

namespace GetraenkeBub
{

    public class RoundChangeUI : MonoBehaviour, IUIState
    {
        [SerializeField] private RectTransform roundChangerUIObject;
        private float screenHeight;
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private TextMeshProUGUI timeText;

        [Header("Movement")]
        [SerializeField] private float movementTime = 0.5f;
        [SerializeField] private LeanTweenType moveInOutTween = LeanTweenType.linear;
        [SerializeField] private float waitTime = 1f;

        [SerializeField] private bool test = false;

        public void Init()
        {
            roundChangerUIObject.gameObject.SetActive(false);
            screenHeight = Screen.currentResolution.height + roundChangerUIObject.rect.height;
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

        private void Update()
        {
            if (test)
            {
                SetRound(4);
                test = false;
            }
        }

        public void SetRound(int round)
        {
            roundChangerUIObject.localPosition = new Vector3(roundChangerUIObject.localPosition.x,-screenHeight,roundChangerUIObject.localPosition.z);
            roundChangerUIObject.gameObject.SetActive(true);
            notificationText.text = round  +" von 9 Stunden Zeit";
            timeText.text = round + ":03 pm";
            TODO.LOCA = false;
            TODO.SOUND = false;
            CancelInvoke();
            LeanTween.cancel(roundChangerUIObject);
            LeanTween.moveLocalY(roundChangerUIObject.gameObject, 0, movementTime).setEase(moveInOutTween).setOnComplete(WaitBeforeMoveOut);
        }

        private void WaitBeforeMoveOut()
        {
            Invoke(nameof(MoveOut), waitTime);
        }

        private void MoveOut()
        {
            CancelInvoke();
            LeanTween.cancel(roundChangerUIObject);
            LeanTween.moveLocalY(roundChangerUIObject.gameObject, screenHeight, movementTime).setEase(moveInOutTween);
        }

        private void End()
        {
            // UIStateManager.Instance.ChangeUIState(EUIState.PlayerUI);
        }
    }
}
