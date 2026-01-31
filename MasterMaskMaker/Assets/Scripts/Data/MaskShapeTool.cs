using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MaskTool", order = 1)]
public class MaskShapeTool : InteractTool
{

    public Sprite MaskBase;
    public override void ToolInteract()
    {
        base.ToolInteract();
        ToolUseHandler.PlaceMask(this);

    }
}
