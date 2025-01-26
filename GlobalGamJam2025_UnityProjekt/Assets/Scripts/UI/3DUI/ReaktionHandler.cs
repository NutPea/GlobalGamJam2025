using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{

    public class ReaktionHandler : MonoBehaviour
    {
        [SerializeField] private Image reaktionImage;

        public void SetImage(Sprite reaktionSprite)
        {
            reaktionImage.sprite = reaktionSprite;
        }
    }
}
