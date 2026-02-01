using UnityEngine;

public class LeanTweenScaleHandler : MonoBehaviour
{

    public float scaleInTime = 0.05f;
    public float scaleOutTime = 0.05f;
    public float scaleAmount = 1.1f;
    public LeanTweenType scaleType = LeanTweenType.easeInOutSine;



    public void StartScale()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleInTime).setEase(scaleType);
    }

    public void EndScale()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), scaleOutTime).setEase(scaleType);
    }

    public void ScaleFeedBack()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleInTime).setEase(scaleType).setOnComplete(EndScale);
    }

}
