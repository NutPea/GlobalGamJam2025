using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInputManager : MonoBehaviour
{
    public static SInputManager Instance;
    public PlayerInput inputActions;

    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Disable();
    }

    private void Awake()
    {
        if(SInputManager.Instance == null)
        {
            Instance = this;
            transform.parent = null;
            inputActions = new();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
