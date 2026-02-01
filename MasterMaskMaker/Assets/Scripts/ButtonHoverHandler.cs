using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LeanTweenScaleHandler))]
public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private LeanTweenScaleHandler leanTweenScaleHandler;
    private void Awake()
    {
        leanTweenScaleHandler = GetComponent<LeanTweenScaleHandler>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        leanTweenScaleHandler.StartScale();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        leanTweenScaleHandler.EndScale();
    }
}
