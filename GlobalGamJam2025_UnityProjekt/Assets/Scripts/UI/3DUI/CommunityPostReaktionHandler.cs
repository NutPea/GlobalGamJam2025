using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class CommunityPostReaktionHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image reaktionImage;


        public void SetReaction(Sprite reaktion , int amount)
        {
            reaktionImage.sprite = reaktion;
            amountText.text = amount.ToString();
        }
    }
}
