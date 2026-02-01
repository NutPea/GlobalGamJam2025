using UnityEngine;

public class LeanTweenScaleHandler : MonoBehaviour
{

    public float scaleTime = 0.25f;
    public float scaleAmount = 1.1f;
    public LeanTweenType scaleType = LeanTweenType.easeInOutSine;



    public void StartScale()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleTime).setEase(scaleType);
    }

    public void EndScale()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), scaleTime).setEase(scaleType);
    }

    public void ScaleFeedBack()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleTime).setEase(scaleType).setOnComplete(StartScale);
    }

}
