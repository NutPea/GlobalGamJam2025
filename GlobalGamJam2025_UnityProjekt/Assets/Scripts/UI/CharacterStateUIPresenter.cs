using Game.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterStateUIPresenter : MonoBehaviour,IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private Image backgroundPicture;
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private Color highlightBackgroundColor = Color.white;

    [SerializeField] private GameObject hasMadeActionOverlay;
    [SerializeField] private Image profilPicture;

     private UnitPresenter thisUnitPresenter;
    private UnitPresenter currentTarget;
    private Transform  lastTarget;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(thisUnitPresenter == currentTarget)
        {
            return;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (thisUnitPresenter == currentTarget)
        {
            return;
        }

    }

    public void Setup(UnitPresenter targetUnitPresenter,UnitPresenter currentTargetPresenter)
    {
        gameObject.SetActive(true);
        thisUnitPresenter = targetUnitPresenter;
        currentTarget = currentTargetPresenter;
        if (currentTargetPresenter == targetUnitPresenter)
        {
            backgroundPicture.color = highlightBackgroundColor;
            profilPicture.sprite = targetUnitPresenter.GetProfilPicture();
            hasMadeActionOverlay.SetActive(false);
        }
        else
        {
            backgroundPicture.color = backgroundColor;
            hasMadeActionOverlay.SetActive(!targetUnitPresenter.GetUsedThisRound());
        }
    }

}
