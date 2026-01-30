using System;
using UnityEngine;
using UnityEngine.UI;

public class MaskUIState : UIState
{
    [SerializeField] private Button backButton;


    public override void OnInit()
    {
        base.OnInit();
        backButton.onClick.AddListener(Back);
    }

    private void Back()
    {
        SUIManager.Instance.ChangeUIState("Customer");
    }
}
