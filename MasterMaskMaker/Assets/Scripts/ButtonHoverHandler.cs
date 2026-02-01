using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LeanTweenScaleHandler))]
public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    private LeanTweenScaleHandler leanTweenScaleHandler;
    private void Awake()
    {
        leanTweenScaleHandler = GetComponent<LeanTweenScaleHandler>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        leanTweenScaleHandler.StartScale();
        SSoundManager.Instance.PlaySound(SSoundManager.Instance.HoverButton);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        leanTweenScaleHandler.EndScale();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SSoundManager.Instance.PlaySound(SSoundManager.Instance.GenericButton);
    }
}
