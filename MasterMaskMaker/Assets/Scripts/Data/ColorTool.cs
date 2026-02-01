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
        if(image.TryGetComponent<ToolHolder>(out ToolHolder holder))
        {
            holder.PlayFeedback();
        }
        if (image.TryGetComponent<MaskUIHandler>(out MaskUIHandler uiHandler))
        {
            uiHandler.Place();
        }

        SGameManager.Instance.AddTool(Instantiate(this));

    }
}
