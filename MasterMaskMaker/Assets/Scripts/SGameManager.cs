using UnityEngine;

public class SGameManager : MonoBehaviour
{
    public SGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
}
