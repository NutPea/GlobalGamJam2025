 using UnityEngine;
public class StartUIState : UIState
{

    public float delay;

    public override void OnEnter()
    {
        base.OnEnter();
        Invoke(nameof(StartGame), delay);
    }

    private void StartGame()
    {
        SGameManager.Instance.StartGame();
    }

}

