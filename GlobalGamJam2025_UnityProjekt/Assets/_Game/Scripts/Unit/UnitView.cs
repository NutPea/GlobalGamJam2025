using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Unit
{
    public class UnitView : AUnitView
    {
        [Header("Color")]
        [SerializeField] private Color red = Color.red;
        [SerializeField] private Color white = Color.white;

        [Header("Life")]
        private int maxHealth = 0;
        [SerializeField] private TextMeshProUGUI _currentLifeText;
        [SerializeField] private Image _lifeImage;


        [Header("ActionPoints")]
        private int maxActionPoints = 0;
        [SerializeField] private TextMeshProUGUI _currentActionText;

        [Header("MovementPoints")]

        private int moveActionPoints = 0;
        [SerializeField] private TextMeshProUGUI _currentMoveText;

        private void Awake()
        {
            _currentActionText.color = white;
            _currentMoveText.color = white;
            _currentLifeText.color = white;
        }

        //HEALTH

        public override void MaxHPChanged(int oldValue, int newValue)
        {
            base.MaxHPChanged(oldValue, newValue);
            maxHealth = newValue;
            SetLife(maxHealth);
        }

        public override void CurrentHPChanged(int oldValue, int newValue)
        {
            base.CurrentHPChanged(oldValue, newValue);
            SetLife(newValue);

            if(newValue == 0)
            {
                GetComponent<UnitPresenter>().DestroyUnit();
            }
        }

        private void SetLife(int newValue)
        {
            float percentag = (float)newValue / (float)maxHealth;
            _lifeImage.fillAmount = percentag;
            _currentLifeText.text = newValue +" / "+ maxHealth;
        }

        //ACTION

        public override void ActionPointChangeModifierChanged(int oldValue, int newValue)
        {
            base.ActionPointChangeModifierChanged(oldValue, newValue);
            if(newValue == 0)
            {
                _currentActionText.color = white;
            }
            else
            {
                _currentActionText.color = red;
            }
        }


        public override void CurrentActionPointsChanged(int oldValue, int newValue)
        {
            base.CurrentActionPointsChanged(oldValue, newValue);
            _currentActionText.text = newValue.ToString();
        }


        //Movement
        public override void MaxMovementPointModifierChanged(int oldValue, int newValue)
        {
            base.MaxMovementPointModifierChanged(oldValue, newValue);
            if (newValue == 0)
            {
                _currentMoveText.color = white;
            }
            else
            {
                _currentMoveText.color = red;
            }
        }
        public override void CurrentMovementPointsChanged(int oldValue, int newValue)
        {
            base.CurrentMovementPointsChanged(oldValue, newValue);
            _currentMoveText.text = newValue.ToString();
        }

    }
}