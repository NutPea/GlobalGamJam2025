using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerHandler : MonoBehaviour
{

    private CustomerData data;
    private Button button;
    public Transform maskPosition;

    [SerializeField] private GameObject funny;
    [SerializeField] private GameObject mysteriouse;
    [SerializeField] private GameObject sparkly;

    public enum customerSize
    {
        small,big,longer
    }

    public customerSize size;

    public void Setup(CustomerData data)
    {
        this.data = data;
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeToCustomer);
        ChooseSparke();
    }

    private void ChooseSparke()
    {
        funny.SetActive(false);
        mysteriouse.SetActive(false);
        sparkly.SetActive(false);
        switch (data.aura)
        {
            case CustomerData.Aura.Funny: funny.SetActive(true);break;
            case CustomerData.Aura.Mysteriouse: mysteriouse.SetActive(true); break;
            case CustomerData.Aura.Sparkly: sparkly.SetActive(true); break;
        }
    }

    private void ChangeToCustomer()
    {
        SGameManager.Instance.SetCustomer(data);
        SSoundManager.Instance.PlaySound(SSoundManager.Instance.UI_OpenWorkbench);
        PlayHappy();
    }

    public float fadeDurationIn = 0.3f;
    public float fadeDurationOut = 0.3f;

    private Image[] images;
    private Coroutine fadeCoroutine;


    public void FadeIn()
    {
        images = GetComponentsInChildren<Image>(true);
        button.interactable = true;
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
            switch (size)
            {
                case customerSize.small: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostLittle_Hello); break;
                case customerSize.longer: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostLong_Hello); break;
                case customerSize.big: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostBig_Hello); break;
            }
       
        });

    }

    public void PlayHappy()
    {
        switch (size)
        {
            case customerSize.small: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostLittle_Happy); break;
            case customerSize.longer: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostLong_Happy); break;
            case customerSize.big: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostBig_Happy); break;
        }
    }

    public void PlaySad()
    {
        switch (size)
        {
            case customerSize.small: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostLittle_Sad); break;
            case customerSize.longer: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostLong_Sad); break;
            case customerSize.big: SSoundManager.Instance.PlaySound(SSoundManager.Instance.NPC_GhostBig_Sad); break;
        }
    }

    public void FadeOut()
    {
        PlayHappy();
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
