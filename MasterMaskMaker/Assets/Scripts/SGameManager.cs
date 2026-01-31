using System.Collections.Generic;
using UnityEngine;

public class SGameManager : MonoBehaviour
{
    public static SGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public CustomerData currentCustomer;
    public List<Tool> UsedTools = new List<Tool>();

    public void AddTool(Tool tool)
    {

    }

    public void TryRemoveTool(Tool tool)
    {
        if(tool == null)
        {
            return;
        }


    }


}
