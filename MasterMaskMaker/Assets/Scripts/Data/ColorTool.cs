using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorTool", order = 1)]
public class ColorTool : InteractTool
{
    public Color Color = Color.red;

    public override void ToolUse(Image image)
    {
        base.ToolUse(image);
        image.color = Color;
        SGameManager.Instance.AddTool(Instantiate(this));
    }
}
