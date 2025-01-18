using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenHandler : MonoBehaviour
{
    [SerializeField] private Image transitionPanel;
    [SerializeField] private GameObject LoadingBackground;
    [SerializeField] private Transform LoadingProgressionPivot;


        
    void Start()
    {
        LoadingBackground.SetActive(false); 
        TransitionOut();
    }


    public void TransitionOut()
    {
        transitionPanel.gameObject.SetActive(true);
        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c = transitionPanel.color;
            c.a = val;
            transitionPanel.color = c;
        }).setOnComplete(StartLoading);
    }

    public void StartLoading()
    {
        LoadingBackground.SetActive(true);
        StartCoroutine(StartLoadingCouroutine());
    }

    IEnumerator StartLoadingCouroutine()
    {
        if(SLoadingManager.ToLoadLevel == -1)
        {
            Debug.LogError("No Scene Index has been set");
        }
        AsyncOperation operation =  SceneManager.LoadSceneAsync(SLoadingManager.ToLoadLevel,LoadSceneMode.Additive);
        while (!operation.isDone)
        {
            SetLoadingProgress(Mathf.Clamp01(operation.progress / 0.9f));
            yield return null;
        }
        TransitionIn();
    }

    public void TransitionIn()
    {
        transitionPanel.gameObject.SetActive(true);
        LoadingBackground.gameObject.SetActive(false);
        LeanTween.value(gameObject, 1, 0, 1).setOnUpdate((float val) =>
        {
            Color c = transitionPanel.color;
            c.a = val;
            transitionPanel.color = c;
        }).setOnComplete(RemoveScene);
    }

    public void RemoveScene()
    {
        SceneManager.UnloadScene(SLoadingManager.Instance.GetSceneIndexByName("LoadingScreen"));
    }

    private void SetLoadingProgress(float progress)
    {
        LoadingProgressionPivot.localScale = new Vector3(progress, 1, 1);
    }
}
