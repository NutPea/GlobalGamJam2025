using UnityEngine;
using UnityEngine.UI;

public class EndDayUIState : UIState
{

    [SerializeField] private Transform ghosts;

    [SerializeField] private Button shareButton;
    [SerializeField] private Button takeScreenShot;
    [SerializeField] private Button quit;


    public override void OnEnter()
    {
        base.OnEnter();
        foreach(GameObject ghost in SGameManager.Instance.copiedCustomers)
        {
            ghost.transform.SetParent(ghosts);
            RectTransform rectTransform = ghost.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            ghost.gameObject.SetActive(true);
        }
    }



}
