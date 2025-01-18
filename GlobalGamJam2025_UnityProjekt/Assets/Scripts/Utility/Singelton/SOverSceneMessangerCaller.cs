using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOverSceneMessangerCaller : MonoBehaviour
{
    public static SOverSceneMessangerCaller Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Action<OverSceneMessangerReceiver>> StartCalls = new();
    public List<Action<OverSceneMessangerReceiver>> LateStartCalls = new();

    public void AddStartCall(Action<OverSceneMessangerReceiver> OnStartCall)
    {
        StartCalls.Add(OnStartCall);
    }

    public void AddLateStartCall(Action<OverSceneMessangerReceiver> OnStartCall)
    {
        LateStartCalls.Add(OnStartCall);
    }

    public void CleanUp()
    {
        StartCalls.Clear();
        LateStartCalls.Clear();
    }

}
