using System;
using UnityEngine;
using UnityEngine.UI;

public class MaskUIState : UIState
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button doneButton;
    private ToolUseHandler toolUseHandler;
    [SerializeField] private ToDoHandler toDoHandler;
    public override void OnInit()
    {
        base.OnInit();
        backButton.onClick.AddListener(Back);
        toolUseHandler = GetComponent<ToolUseHandler>();
        toolUseHandler.Init();
        doneButton.onClick.AddListener(MaskGive);
    }

    private void MaskGive()
    {

        SGameManager.Instance.GiveMask();
    }

    public override void OnBeforeEnter()
    {
        base.OnBeforeEnter();
        toolUseHandler.OnEnter();
        toDoHandler.OnEnter();
    }

    private void Back()
    {
        SUIManager.Instance.ChangeUIState("Customer");
    }
}
