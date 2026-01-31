using System;
using UnityEngine;
using UnityEngine.UI;

public class MaskUIState : UIState
{
    [SerializeField] private Button backButton;
    private ToolUseHandler toolUseHandler;

    public override void OnInit()
    {
        base.OnInit();
        backButton.onClick.AddListener(Back);
        toolUseHandler = GetComponent<ToolUseHandler>();
        toolUseHandler.Init();
    }

    public override void OnBeforeEnter()
    {
        base.OnBeforeEnter();
        toolUseHandler.OnEnter();
    }

    private void Back()
    {
        SUIManager.Instance.ChangeUIState("Customer");
    }
}
