using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndDayUIState : UIState
{

    [SerializeField] private Transform ghosts;

    [SerializeField] private Button shareButton;
    [SerializeField] private Button takeScreenShot;
    [SerializeField] private Button quit;

    [SerializeField] private TextMeshProUGUI whereToFind;
    private string path;

    public override void OnInit()
    {
        base.OnInit();
        shareButton.onClick.AddListener(ShareintendInstagram);
        takeScreenShot.onClick.AddListener(TakeScreenShot);
        quit.onClick.AddListener(Quit);
    }

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
        whereToFind.gameObject.SetActive(false);
        SSoundManager.Instance.PlaySound(SSoundManager.Instance.UI_Endscore);
    }

    private void SetActive(bool value)
    {
        shareButton.gameObject.SetActive(value);
        takeScreenShot.gameObject.SetActive(value);
        quit.gameObject.SetActive(value);
        whereToFind.gameObject.SetActive(value);
    }

    private void ShowInfo()
    {
        whereToFind.gameObject.SetActive(true);
        whereToFind.text = "You can find you Screenshot here" + path; 
    }

    private void TakeScreenShot()
    {
        StartCoroutine(Capture());
    }


    IEnumerator Capture()
    {
        SetActive(false);
        yield return new WaitForSeconds(0.1f);

        path = System.IO.Path.Combine(
            Application.persistentDataPath,
            "screenshot_" + System.DateTime.Now.Ticks + ".png"
        );

        ScreenCapture.CaptureScreenshot(path);
        Debug.Log("Screenshot gespeichert: " + path);
        yield return new WaitForSeconds(0.1f);
        ShowInfo();
        SetActive(true);
    }



    private void ShareintendInstagram()
    {
        GUIUtility.systemCopyBuffer = path;
    }


    private void ShareintendBlueSky()
    {

    }

    private void Quit()
    {
        Application.Quit();
    }



}
