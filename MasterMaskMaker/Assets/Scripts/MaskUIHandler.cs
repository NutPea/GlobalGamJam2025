using UnityEngine;
using UnityEngine.EventSystems;

public class MaskUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool IsOverMask;
    public Tool Tool;

    private void Update()
    {
        
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        IsOverMask = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsOverMask = false;
    }
}
