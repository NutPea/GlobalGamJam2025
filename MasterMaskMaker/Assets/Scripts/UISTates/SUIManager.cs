using System;
using System.Collections.Generic;
using UnityEngine;

public class SUIManager : MonoBehaviour
{
    public static SUIManager Instance;
    public Canvas mainCanvas;

    [SerializeField] private List<State> UIStates = new List<State>();
    private Dictionary<string, State> UIStateDictonary = new Dictionary<string, State>();
    public State CurrentUIState;

    [SerializeField] private bool startWithUIState;
    [SerializeField] private string startUiStateName;

    [SerializeField] private UITransition transition;

    private void Awake()
    {
        Instance = this;
        foreach(State state in UIStates)
        {
            UIStateDictonary.Add(state.Name, state);
        }
    }

    private void Start()
    {
        foreach (State state in UIStates)
        {
            state.stateObject.gameObject.SetActive(true);
            state.OnInit();
            state.stateObject.gameObject.SetActive(false);
        }

        if (startWithUIState)
        {
            ChangeUIState(startUiStateName);
        }
        transition.OnTransitionDone.AddListener(ChangeToUIState);
    }

    private string toChangeUIStateName = "";

    public void ChangeUIState(string name)
    {
        toChangeUIStateName = name;
        transition.StartTransition();
    }

    private void ChangeToUIState()
    {
        State toChangeState = UIStateDictonary[toChangeUIStateName];
        if (CurrentUIState.stateObject != null)
        {
            CurrentUIState.Exit();
            CurrentUIState.stateObject.gameObject.SetActive(false);
        }
        CurrentUIState = toChangeState;
        CurrentUIState.stateObject.gameObject.SetActive(true);
        toChangeState.OnBeforeEnter();
        toChangeState.Enter();
    }

}

[System.Serializable]
public class State
{
    public UIState stateObject;
    public string Name;


    public void OnInit()
    {
        stateObject.OnInit();
    }

    public void OnBeforeEnter()
    {
        stateObject.OnBeforeEnter();
    }

    public void Enter()
    {
        stateObject.OnEnter();
    }

    public void Exit()
    {

        stateObject.OnExit();
    }
}