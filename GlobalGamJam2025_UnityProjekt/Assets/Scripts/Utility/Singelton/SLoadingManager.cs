using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SLoadingManager : MonoBehaviour
{

    public static SLoadingManager Instance;

    private static int _toLoadLevel;
    public static int ToLoadLevel => _toLoadLevel;
    public UnityEvent OnCleanup = new();

    public enum LevelName
    {
        Loading_Screen = -2,
        None = -1,
        LoadingScreen = 0,
        MainMenu = 1,
        MechConfig = 2,
        PrototypLevel = 100,
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadScene(string sceneName)
    {
        LoadScene(GetSceneIndexByName(sceneName));
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetSceneIndexByName(string sceneName)
    {
        int sceneNumber = SceneManager.sceneCountInBuildSettings;
        for(int possibleSceneIndex = 0; possibleSceneIndex < sceneNumber; possibleSceneIndex++)
        {
            string scenePath = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(possibleSceneIndex));
            int slash = scenePath.LastIndexOf('/');
            string name = scenePath.Substring(slash + 1);

            if(name == sceneName){
                return possibleSceneIndex;
            }
        }

        Debug.LogError($"You cant load the Scene Name : {sceneName}");
        return -1;
    }

    public void LoadScene(LevelName levelName)
    {
        switch (levelName)
        {
            case LevelName.MainMenu: LoadScene("MainMenu"); break;
            case LevelName.MechConfig: LoadScene("MechConfiguration"); break;
            case LevelName.PrototypLevel: LoadScene("PROTOTYPLevel_Kanalisation"); break;
        }
    }


    public void LoadScene(int sceneIndex)
    {
        if(sceneIndex == -1)
        {
            Debug.LogError($"You cant load the index : {sceneIndex}");
        }
        OnCleanup.Invoke();
        _toLoadLevel = sceneIndex;
        SceneManager.LoadScene(GetSceneIndexByName("LoadingScreen"));
    }
    



}
