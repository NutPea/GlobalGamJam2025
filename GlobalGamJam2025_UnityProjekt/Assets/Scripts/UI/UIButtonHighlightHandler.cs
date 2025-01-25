using UnityEngine;
using UnityEngine.EventSystems;

namespace GetraenkeBub {
    public class UIButtonHighlightHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float scaleAmount = 1.1f;
        [SerializeField] private float scaleTime = 0.5f;
        [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;

        public void OnPointerEnter(PointerEventData eventData)
        {
           LeanTween.cancel(gameObject);
           LeanTween.scale(gameObject,new Vector3(scaleAmount,scaleAmount,transform.localScale.z),scaleTime).setEase(tweenType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject,Vector3.one, scaleTime).setEase(tweenType);
        }
    }
}
