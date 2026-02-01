using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolUseHandler : MonoBehaviour
{
    [SerializeField] private List<ToolButton> toolButtons;


    private Tool CurrentUsedTool;
    private InteractTool currentInteractTool;
    private DragTool currentDragTool;

    private bool HasTool => CurrentUsedTool != null;
    private bool IsInteractTool => currentInteractTool != null;

    private bool canBeDragged;

    [Header("Category")]
    [SerializeField] private Button maskCategoryButton;
    [SerializeField] private List<Tool> masksTools;


    [SerializeField] private Button secondaryCategoryButton;
    [SerializeField] private List<Tool> secondaryTools;

    [SerializeField] private Button minorToolsCategoryButton;
    [SerializeField] private List<Tool> minorTools;

    [SerializeField] private Button colorCategoryButton;
    [SerializeField] private List<Tool> colorTools;

    [SerializeField] private Button deleteButton;

    [SerializeField]private Transform schublade;
    [SerializeField] private float moveTime;
    [SerializeField] private float moveAmount;
    [SerializeField] private LeanTweenType moveType = LeanTweenType.easeInOutSine;

    private float schubladeXPosition;

    [Header("Mask")]

    private PlayerInput playerInput;

    [SerializeField] private Transform zwischenMaskTransform;
    [SerializeField] private Transform maskTransform;
    [SerializeField] private Image maskImage;
    private MaskUIHandler maskUIHandler;


    private GameObject spawnedDragable;
    private RectTransform spawnableRectTransform;
    private bool IsDragging;

    [SerializeField]float rotationSpeed = 1f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Keyboard.LeftMouseButton.performed += ctx => UseTool();
        playerInput.Keyboard.LeftMouseButton.canceled += ctx => MouseButtonUp();

        playerInput.Keyboard.RightMouseButton.performed += ctx => RemoveTool();

        maskCategoryButton.onClick.AddListener(() => SetCategory(masksTools));

        secondaryCategoryButton.onClick.AddListener(() => SetCategory(secondaryTools));

        minorToolsCategoryButton.onClick.AddListener(() => SetCategory(minorTools));

        colorCategoryButton.onClick.AddListener(() => SetCategory(colorTools));
        schubladeXPosition = schublade.GetComponent<RectTransform>().localPosition.x;

        playerInput.Keyboard.Scroll.performed += ctx => Scroll();
        playerInput.Keyboard.Scroll.canceled += ctx => Scroll();

        deleteButton.onClick.AddListener(DeleteAll);
    }

    private void Scroll()
    {
        if (!IsDragging)
        {
            return;
        }
        float scrollAmount = playerInput.Keyboard.Scroll.ReadValue<float>();
        spawnableRectTransform.Rotate(new Vector3(0, 0, scrollAmount * rotationSpeed),Space.Self);
        
    }

    private void Start()
    {
        
        SGameManager.Instance.OnChangeCustomer.AddListener(DeleteAll);
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

        SetCategoryImmediate(masksTools);
        maskUIHandler = maskTransform.GetComponent<MaskUIHandler>();
    }

    public void OnEnter()
    {
        maskTransform.gameObject.SetActive(false);
        
    }

    private List<Tool> toChangeToTools = new List<Tool>();

    public void SetCategory(List<Tool> categoryTools)
    {
        toChangeToTools = categoryTools;
        LeanTween.moveLocalX(schublade.gameObject, -moveAmount, moveTime).setEase(moveType).setOnComplete(ChangeCategory);

    }

    private void ChangeCategory()
    {
        SetCategoryImmediate(toChangeToTools);
        LeanTween.moveLocalX(schublade.gameObject, schubladeXPosition, moveTime).setEase(moveType);
    }

    public void SetCategoryImmediate(List<Tool> categoryTools)
    {
        foreach (ToolButton toolButton in toolButtons)
        {
            toolButton.RemoveTool();
        }

        for (int i = 0; i< categoryTools.Count; i++)
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

    private void DeleteAll()
    {
        for (int i = maskTransform.childCount - 1; i >= 0; i--)
        {
            GameObject child = maskTransform.GetChild(i).gameObject;
            Destroy(child);
        }
        SGameManager.Instance.RemoveAll();
        maskTransform.gameObject.SetActive(false);
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

    private void RemoveTool()
    {
        Image image = SearchForImage();
        if(image == null)
        {
            return;
        }

        if(image.TryGetComponent<ToolHolder>(out ToolHolder holder))
        {
            SGameManager.Instance.TryRemoveTool(holder.Tool);
            Destroy(holder.gameObject);
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

        spawnedDragable.transform.parent = maskTransform;
        zwischenMaskTransform.gameObject.SetActive(false);

        if (!IsOverMask())
        {
            Destroy(spawnedDragable);
            OnNotOverMask();
            
        }
        else
        {
            ToolHolder toolHolder = spawnedDragable.GetComponent<ToolHolder>();
            SGameManager.Instance.AddTool(toolHolder.Tool);
            toolHolder.PlayFeedback();
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
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        List<Transform> transforms = new List<Transform>();
       
        foreach(RaycastResult resultTransforms in results)
        {
            transforms.Add(resultTransforms.gameObject.transform);
        }

        return transforms.Contains(maskTransform);
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
        maskUIHandler.Place();

    }
    public void SpawnDragable(DragTool dragTool)
    {
        spawnedDragable = Instantiate(dragTool.SpawnedDragTool);
        zwischenMaskTransform.gameObject.SetActive(true);
        spawnedDragable.transform.parent = zwischenMaskTransform;
        spawnableRectTransform = spawnedDragable.GetComponent<RectTransform>();
        spawnableRectTransform.position = Vector3.zero;
        spawnableRectTransform.localScale = Vector3.one;

        ToolHolder toolHolder = spawnedDragable.GetComponent<ToolHolder>();
        toolHolder.Tool = dragTool;
        IsDragging = true;
    }

}
