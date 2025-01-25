using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Game.GameModel;

namespace GetraenkeBub
{

    public class AbilityButtonView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private AAbility ability;
        private Button button;
        public AAbility AAbility {
            set => ability = value;
        }

        [SerializeField] private TextMeshProUGUI abilityText;
        [SerializeField] private Image abilityImage;
        [SerializeField] private TextMeshProUGUI abilityCostText;
        [SerializeField] private GameObject overlay;

        [Header("Colors")]
        public Color toLessColor = Color.red;
        public Color normalColor = Color.white;

        public void SetAbility(AAbility toSetAbility, AbilityUsability abilityUsability)
        {
            ability = toSetAbility;
            if(toSetAbility.abilityIcon == null)
            {
                abilityText.text = toSetAbility.AbilityName;
                abilityText.gameObject.SetActive(true);
                abilityImage.gameObject.SetActive(false);
            }
            else
            {
                abilityText.gameObject.SetActive(false);
                abilityImage.gameObject.SetActive(true);
                abilityImage.sprite = toSetAbility.abilityIcon;
            }
            switch (abilityUsability)
            {
                case AbilityUsability.Castable: abilityCostText.color = normalColor; overlay.SetActive(false);  break;
                case AbilityUsability.NotEnoughEnergy: abilityCostText.color = toLessColor; overlay.SetActive(true); break;
                case AbilityUsability.TargetNotAvailable: abilityCostText.color = toLessColor; overlay.SetActive(true); break;
            }

            abilityCostText.text = ability.actionPointCost.ToString();

        }

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(CastAbility);
        }

        private void CastAbility()
        {
            if(!gameObject.activeSelf || ability == null || overlay.activeSelf)
            {
                return;
            }
            UIStateManager.Instance.InvokeOnAbilityCasted(ability);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!gameObject.activeSelf || ability == null || overlay.activeSelf)
            {
                return;
            }
            UIStateManager.Instance.InvokeOnAbilityHighlight(ability);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!gameObject.activeSelf || ability == null || overlay.activeSelf)
            {
                return;
            }
            UIStateManager.Instance.InvokeOnAbilityStop();
        }
    }
}
