using FMODUnity;
using UnityEngine;

public class SSoundManager : MonoBehaviour
{
    public static SSoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    [EventRef]
    public string GenericButton;

    [EventRef]
    public string HoverButton;

    [EventRef] 
    public string UI_Trash;

    [EventRef]
    public string UI_SellMask;

    [EventRef]
    public string UI_Scissors;


    [EventRef]
    public string UI_Endscore;

    [EventRef]
    public string UI_DropMaskshape;

    [EventRef]
    public string UI_DropItem;

    [EventRef]
    public string UI_DropColor;

    [EventRef]
    public string UI_ClickCategory;

    [EventRef]
    public string UI_10SecTimer;
    [EventRef]
    public string NPC_GhostSpawning;
    [EventRef]
    public string NPC_GhostLong_Sad;
    [EventRef]
    public string NPC_GhostLong_Hello;
    [EventRef]
    public string NPC_GhostLong_Happy;

    [EventRef]
    public string NPC_GhostLittle_Sad;
    [EventRef]
    public string NPC_GhostLittle_Hello;
    [EventRef]
    public string NPC_GhostLittle_Happy;

    [EventRef]
    public string NPC_GhostBig_Hello;
    [EventRef]
    public string NPC_GhostBig_Happy;
    [EventRef]
    public string NPC_GhostBig_Sad;


    public void PlaySound(string path)
    {
        RuntimeManager.PlayOneShot(path);
    }
}
