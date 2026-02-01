using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskUIState : UIState
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button doneButton;
    private ToolUseHandler toolUseHandler;
    [SerializeField] private ToDoHandler toDoHandler;

    [SerializeField] private TextMeshProUGUI timer;
    private float currentTime = 0f;
    private bool hasTime = false;

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
        SSoundManager.Instance.PlaySound(SSoundManager.Instance.UI_SellMask);
    }

    private void Update()
    {
        if (hasTime)
        {
            if(currentTime < 0)
            {
                MaskGive();
                hasTime = false;
            }
            else
            {
                currentTime -= Time.deltaTime;
                if(currentTime < 10)
                {
                    timer.color = Color.red;
                }
                timer.text = currentTime.ToString("F0");
            }

        }   
    }

    public override void OnBeforeEnter()
    {
        base.OnBeforeEnter();
        toolUseHandler.OnEnter();
        toDoHandler.OnEnter();
        hasTime = false;
        CustomerData data = SGameManager.Instance.currentCustomer;
        if(data != null)
        {
            hasTime = true;
            currentTime = data.time;

        }
        timer.color = Color.black;
    }

    private void Back()
    {
        SUIManager.Instance.ChangeUIState("Customer");
    }
}
