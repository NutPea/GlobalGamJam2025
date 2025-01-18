using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverSceneMessangerReceiver : MonoBehaviour
{
    public GameObject player;

    private void Awake()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    IEnumerator Start()
    {
        foreach(Action<OverSceneMessangerReceiver> startCall in SOverSceneMessangerCaller.Instance.StartCalls)
        {
            startCall.Invoke(this);
        }
        yield return new WaitForEndOfFrame();

        foreach (Action<OverSceneMessangerReceiver> lateStartCall in SOverSceneMessangerCaller.Instance.LateStartCalls)
        {
            lateStartCall.Invoke(this);
        }

        SOverSceneMessangerCaller.Instance.CleanUp();

    }

}
