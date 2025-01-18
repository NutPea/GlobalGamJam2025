using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{


    public void B_ChangeToPlay()
    {
        SLoadingManager.Instance.LoadScene(SLoadingManager.LevelName.Test);
    }
}
