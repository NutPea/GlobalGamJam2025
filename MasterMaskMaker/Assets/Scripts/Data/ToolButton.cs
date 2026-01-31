using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour , IPointerDownHandler
{
    [SerializeField] private Tool Tool;

    [SerializeField] private Image image;
    public UnityEvent<Tool> OnInteract;

    public void Init()
    {
 

    }

    public void SetUpButton(Tool tool, ToolUseHandler toolUseHandler)
    {
        Tool =Instantiate(tool);
        image.sprite = tool.Icon;
        if(Tool is ColorTool colorTool)
        {
            image.color = colorTool.Color;
        }
        else
        {
            image.color = Color.white;
        }
        Tool.Init(toolUseHandler);
    }

    public void RemoveTool()
    {
        Tool = null;
    }

    public void SetActive()
    {
        gameObject.SetActive(Tool != null);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        OnInteract.Invoke(Tool);
    }
}
