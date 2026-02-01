using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerHandler : MonoBehaviour
{

    private CustomerData data;
    private Button button;
    public Transform maskPosition;

    public void Setup(CustomerData data)
    {
        this.data = data;
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeToCustomer);
    }

    private void ChangeToCustomer()
    {
        SGameManager.Instance.SetCustomer(data);
    }

    public float fadeDurationIn = 0.3f;
    public float fadeDurationOut = 0.3f;

    private Image[] images;
    private Coroutine fadeCoroutine;


    public void FadeIn()
    {
        images = GetComponentsInChildren<Image>(true);
        button.interactable = false;
        LeanTween.value(gameObject, 0, 1, fadeDurationOut).setOnUpdate((val) =>
        {
            foreach (Image i in images)
            {
                Color c = i.color;
                c.a = val;
                i.color = c;
            }
        }).setOnComplete(() =>
        {
            button.interactable = true;

        });

    }

    public void FadeOut()
    {
        images = GetComponentsInChildren<Image>(true);
        LeanTween.value(gameObject, 1, 0, fadeDurationOut).setOnUpdate((val) =>
        {
            foreach(Image i in images)
            {
                Color c = i.color;
                c.a = val;
                i.color = c;
            }
        });

        button.interactable = false;
    }

    

}
