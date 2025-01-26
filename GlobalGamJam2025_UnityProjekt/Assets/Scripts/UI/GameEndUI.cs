using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class GameEndUI : MonoBehaviour,IUIState
    {
        [SerializeField] private TextMeshProUGUI pointAmount;

        [SerializeField] private Button restartButton;
        [SerializeField] private Button closeButton;

        public void Init()
        {
            restartButton.onClick.AddListener(() => UIStateManager.Instance.GetBackToMenu());
            closeButton.onClick.AddListener(() => UIStateManager.Instance.CloseGame());
        }



        public void OnBeforeEnter()
        {
            pointAmount.text = "You received :" + UIStateManager.Instance.AmountOfPoints.ToString() + "Follower!";
        }

        public void OnEnter()
        {

        }

        public void OnLeave()
        {

        }
    }
}
