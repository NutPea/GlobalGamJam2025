using Unity.VisualScripting;
using UnityEngine;

namespace Game.Grid
{
    [ExecuteInEditMode]
    public abstract class AGridContent : MonoBehaviour
    {
        [SerializeField] private GameObject debugVisual;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                debugVisual.SetActive(false);
            }
        }

        private void Update()
        {
            transform.position = Vector3Int.RoundToInt(transform.position);
        }
    }
}