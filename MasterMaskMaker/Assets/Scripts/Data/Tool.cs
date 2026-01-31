using System;
using UnityEngine;

public class Tool : ScriptableObject
{
    public Sprite Icon;
    public string GUID;

    protected ToolUseHandler ToolUseHandler;
    public virtual void Init(ToolUseHandler toolUseHandler)
    {
        this.ToolUseHandler = toolUseHandler;
    }
}
