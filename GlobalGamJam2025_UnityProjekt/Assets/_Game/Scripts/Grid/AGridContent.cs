using Unity.VisualScripting;
using UnityEngine;

namespace Game.Grid
{
    [ExecuteInEditMode]
    public abstract class AGridContent : MonoBehaviour
    {
        public enum HighlightOption { None, Movement, Ability}

        [SerializeField] private GameObject debugVisual;
        private HighlightOption currentHighlightOption;

        private GameObject movementHighlight;
        private GameObject abilityHighlight;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                debugVisual?.SetActive(false);
            }
        }

        private void Update()
        {
            transform.position = Vector3Int.RoundToInt(transform.position);
        }

        public void SetHighlightOption(HighlightOption highlightOption)
        {
            currentHighlightOption = highlightOption;

            transform.Find("MH")?.gameObject.SetActive(highlightOption == HighlightOption.Movement);
            transform.Find("AH")?.gameObject.SetActive(highlightOption == HighlightOption.Ability);
        }
    }
}