using Game;
using TMPro;
using UnityEngine;

namespace GetraenkeBub {
    public class AbilityDescription : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI abilityNameText;
        [SerializeField] private TextMeshProUGUI abilityDescriptionText;

        private void Start()
        {
            UIStateManager.Instance.OnAbilityHighlight += ShowAbility;
            UIStateManager.Instance.OnAbilityStop += HideAbility;
            gameObject.SetActive(false);


        }

        public void ShowAbility(AAbility ability)
        {
            abilityNameText.text = ability.AbilityName;
            abilityDescriptionText.text = ability.AbilityDescription;
            gameObject.SetActive(true);
        }

        public void HideAbility()
        {
            gameObject.SetActive(false);
        }

    }
}
