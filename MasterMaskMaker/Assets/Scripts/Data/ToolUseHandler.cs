using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolUseHandler : MonoBehaviour
{
    [SerializeField] private List<ToolButton> toolButtons;

    [SerializeField] private List<Tool> category1;

    private Tool CurrentUsedTool;
    private InteractTool currentInteractTool;
    private DragTool currentDragTool;

    private bool HasTool => CurrentUsedTool != null;
    private bool IsInteractTool => currentInteractTool != null;

    private bool canBeDragged;

    [Header("Mask")]

    private PlayerInput playerInput;


    [SerializeField] private Transform maskTransform;
    [SerializeField] private Image maskImage;
    private MaskUIHandler maskUIHandler;


    private GameObject spawnedDragable;
    private RectTransform spawnableRectTransform;
    private bool IsDragging;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Keyboard.LeftMouseButton.performed += ctx => UseTool();
        playerInput.Keyboard.LeftMouseButton.canceled += ctx => MouseButtonUp();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void Init()
    {
        foreach(ToolButton toolButton in toolButtons)
        {
            toolButton.Init();
            toolButton.OnInteract.AddListener(Interact);
        }

        SetCategory(category1);
        maskUIHandler = maskTransform.GetComponent<MaskUIHandler>();
    }

    public void OnEnter()
    {
        maskTransform.gameObject.SetActive(false);
        
    }

    public void SetCategory(List<Tool> categoryTools)
    {
        for(int i = 0; i< categoryTools.Count; i++)
        {
            toolButtons[i].SetUpButton(categoryTools[i],this);
        }

        foreach (ToolButton toolButton in toolButtons)
        {
            toolButton.SetActive();
        }
    }

    public void Interact(Tool tool)
    {
        ClearTool();
        CurrentUsedTool = tool;
        if(CurrentUsedTool is InteractTool interactTool)
        {
            currentInteractTool = interactTool;
            interactTool.ToolInteract();
        }else if(CurrentUsedTool is DragTool dragTool)
        {
            currentDragTool = dragTool;
            SpawnDragable(dragTool);
        }
    }

    public void UseTool()
    {
        if (!HasTool)
        {
            return;
        }

        if (IsOverMask())
        {
            currentInteractTool.ToolUse(SearchForImage());
        }
        else
        {
            OnNotOverMask();
        }
    }

    private Image SearchForImage()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        Image foundImage = results[0].gameObject.GetComponent<Image>();
        return foundImage;
    }

    private void Update()
    {
        if (IsDragging)
        {
            MouseDrag();
        }
    }

    public void MouseDrag()
    {
        if (!HasTool)
        {
            return;
        }

        if (IsInteractTool)
        {
            return;
        }

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            SUIManager.Instance.mainCanvas.transform as RectTransform,
            Input.mousePosition,
            SUIManager.Instance.mainCanvas.worldCamera,
            out mousePos
        );

        spawnableRectTransform.localPosition = mousePos;

    }

    public void MouseButtonUp()
    {

        IsDragging = false;
        if (!HasTool)
        {
            return;
        }
        if (IsInteractTool)
        {
            return;
        }


        if (!IsOverMask())
        {
            Destroy(spawnedDragable);
            OnNotOverMask();
        }
        ClearTool();
    }

    private void ClearTool()
    {
        CurrentUsedTool = null;
        currentDragTool = null;
        currentInteractTool = null;
    }



    private bool IsOverMask()
    {
        return maskUIHandler.IsOverMask;
    }

    private void OnNotOverMask()
    {

    }

    public void PlaceMask(MaskShapeTool maskShapeTool)
    {
        maskTransform.gameObject.SetActive(true);
        maskImage.sprite = maskShapeTool.MaskBase;
        SGameManager.Instance.TryRemoveTool(maskUIHandler.Tool);
        maskUIHandler.Tool = Instantiate(maskShapeTool);

    }
    public void SpawnDragable(DragTool dragTool)
    {
        spawnedDragable = Instantiate(dragTool.SpawnedDragTool);
        spawnedDragable.transform.parent = maskTransform;
        spawnableRectTransform = spawnedDragable.GetComponent<RectTransform>();
        spawnableRectTransform.position = Vector3.zero;
        IsDragging = true;
    }

}
