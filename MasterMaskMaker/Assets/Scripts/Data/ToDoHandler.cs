using UnityEngine;
using UnityEngine.UI;

public class ToDoHandler : MonoBehaviour
{

    [SerializeField] private Image maskImage;
    [SerializeField] private Image secondImage;

    public void OnEnter()
    {
        CustomerData data = SGameManager.Instance.currentCustomer;
        if(data == null)
        {
            maskImage.gameObject.SetActive(false);
            secondImage.gameObject.SetActive(false);
            return;
        }

        maskImage.gameObject.SetActive(true);
        secondImage.gameObject.SetActive(true);

        maskImage.sprite = data.mask.Icon;
        secondImage.sprite = data.minorDetail.Icon; 
    }

}
