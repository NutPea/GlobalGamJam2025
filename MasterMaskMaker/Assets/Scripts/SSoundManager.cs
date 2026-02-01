using FMODUnity;
using UnityEngine;

public class SSoundManager : MonoBehaviour
{
    public static SSoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }


   
    public FMODUnity.EventReference GenericButton;


    public FMODUnity.EventReference HoverButton;


    public FMODUnity.EventReference UI_Trash;


    public FMODUnity.EventReference UI_SellMask;

    public FMODUnity.EventReference UI_Scissors;



    public FMODUnity.EventReference UI_Endscore;

    public FMODUnity.EventReference UI_DropMaskshape;


    public FMODUnity.EventReference UI_OpenWorkbench;


    public FMODUnity.EventReference UI_DropItem;


    public FMODUnity.EventReference UI_DropColor;


    public FMODUnity.EventReference UI_ClickCategory;


    public FMODUnity.EventReference UI_10SecTimer;

    public FMODUnity.EventReference NPC_GhostSpawning;

    public FMODUnity.EventReference NPC_GhostLong_Sad;

    public FMODUnity.EventReference NPC_GhostLong_Hello;

    public FMODUnity.EventReference NPC_GhostLong_Happy;


    public FMODUnity.EventReference NPC_GhostLittle_Sad;

    public FMODUnity.EventReference NPC_GhostLittle_Hello;

    public FMODUnity.EventReference NPC_GhostLittle_Happy;


    public FMODUnity.EventReference NPC_GhostBig_Hello;

    public FMODUnity.EventReference NPC_GhostBig_Happy;

    public FMODUnity.EventReference NPC_GhostBig_Sad;


    public void PlaySound(FMODUnity.EventReference path)
    {
        RuntimeManager.PlayOneShot(path);
    }
}
