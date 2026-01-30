using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITransition : MonoBehaviour
{
    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionTime;

    public LeanTweenType transitionType;

    public UnityEvent OnTransitionDone = new UnityEvent();

    public void StartTransition()
    {
        transitionImage.gameObject.SetActive(true);
        SetAlpha(0f);
        LeanTween.value(gameObject, 0, 1, transitionTime).setOnUpdate(SetAlpha).setOnComplete(DoneTransition);
    }

    private void DoneTransition()
    {
        OnTransitionDone.Invoke();
        EndTransition();
    }

    private void SetAlpha(float value)
    {
        Color newIMageValue = transitionImage.color;
        newIMageValue.a = value;
        transitionImage.color = newIMageValue;
    }

    public void EndTransition()
    {
        LeanTween.value(gameObject, 1, 0, transitionTime).setOnUpdate(SetAlpha).setOnComplete(HideImage);
    }

    private void HideImage()
    {
        transitionImage.gameObject.SetActive(false);
    }

}
