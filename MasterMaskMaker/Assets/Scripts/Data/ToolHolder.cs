using UnityEngine;

public class ToolHolder : MonoBehaviour
{

    private LeanTweenScaleHandler leanTweenScaleHandler;
    public Tool Tool;

    private void Start()
    {
        leanTweenScaleHandler = GetComponent<LeanTweenScaleHandler>();
    }

    public void PlayFeedback()
    {
        leanTweenScaleHandler.ScaleFeedBack();
    }
}
