using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DragTool", order = 1)]
public class DragTool : Tool
{

    public GameObject SpawnedDragTool;
    public virtual void StartDrag()
    {

    }

    public virtual void OnDrag()
    {

    }

    public virtual void EndDrag()
    {

    }
}
