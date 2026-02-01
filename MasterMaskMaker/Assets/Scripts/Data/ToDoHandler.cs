using UnityEngine;
using UnityEngine.UI;

public class ToDoHandler : MonoBehaviour
{

    [SerializeField] private Image maskImage;
    [SerializeField] private Image secondImage;
    [SerializeField] private Image secondImage2;

    public void OnEnter()
    {
        Invoke(nameof(Set), 0.5f);
    }

    private void Set()
    {
        CustomerData data = SGameManager.Instance.currentCustomer;
        if (data == null)
        {
            maskImage.gameObject.SetActive(false);
            secondImage.gameObject.SetActive(false);
            return;
        }

        maskImage.gameObject.SetActive(true);
        secondImage.gameObject.SetActive(true);

        if(data.mask.Icon != null)
        {
            maskImage.sprite = data.mask.Icon;
        }

        if (data.secondaryDetail.Icon != null)
        {
            secondImage.sprite = data.secondaryDetail.Icon;
        }

        if (data.secondaryDetail2.Icon != null)
        {
            secondImage2.sprite = data.secondaryDetail2.Icon;
        }
    }

}
