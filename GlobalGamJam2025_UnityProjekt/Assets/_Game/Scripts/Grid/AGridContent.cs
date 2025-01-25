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

            movementHighlight = transform.Find("MH").gameObject;
            abilityHighlight = transform.Find("AH").gameObject;
        }

        private void Update()
        {
            transform.position = Vector3Int.RoundToInt(transform.position);
        }

        public void SetHighlightOption(HighlightOption highlightOption)
        {
            currentHighlightOption = highlightOption;

            movementHighlight?.SetActive(highlightOption == HighlightOption.Movement);
            abilityHighlight?.SetActive(highlightOption == HighlightOption.Ability);
        }
    }
}