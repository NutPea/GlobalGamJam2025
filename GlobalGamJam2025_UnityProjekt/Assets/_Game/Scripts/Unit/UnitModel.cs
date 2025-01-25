using UnityEngine;

namespace Game.Unit
{
    public class UnitModel : MonoBehaviour
    {
        [SerializeField, Tooltip("maximale movement aktionen pro Runde")] private int _maxMovePoints;

        [SerializeField, Tooltip("Actionpoints in der ersten Runde BEVOR defaultActionPointChange applied wurde")] private int _startActionPoints;
        [SerializeField, Tooltip("Wird am Rundenbeginn auf die aktuellen ActionPoints drauf gerechnet, kann durch Fähigkeiten temporär beinflusst werden")] private int _defaultActionPointChange;
        [SerializeField, Tooltip("Maximale Action points der Unit")] private int _maxActionPoints;

        [SerializeField, Tooltip("Maximale und StartHP der Unit")] private int _maxHP;
        [SerializeField, Tooltip("Initiative, um Reihenfolge der Units zu definieren")] private int _initiative;


        //HP
        [HideInInspector] public ModelEntry<int> MaxHP = new();
        [HideInInspector] public ModelEntry<int> CurrentHP = new();

        //MOVEMENT
        [HideInInspector] public ModelEntry<Vector2Int> Position = new();
        [HideInInspector] public ModelEntry<int> MaxMovementPoints = new();
        [HideInInspector] public ModelEntry<int> CurrentMovementPoints = new();
        [HideInInspector] public ModelEntry<int> MaxMovementPointModifier = new();

        //ACTION POINTS
        [HideInInspector] public ModelEntry<int> CurrentActionPoints = new();
        [HideInInspector] public ModelEntry<int> ActionPointChange = new();
        [HideInInspector] public ModelEntry<int> ActionPointChangeModifier = new();
        [HideInInspector] public ModelEntry<int> MaxActionPoints = new();


        [HideInInspector] public ModelEntry<bool> IsUsedThisRound = new();

        private void Awake()
        {
            InitValues();
        }

        private void InitValues()
        {
            MaxHP.Value = _maxHP;
            CurrentHP.Value = _maxHP;

            MaxMovementPoints.Value = _maxMovePoints;
            CurrentMovementPoints.Value = _maxMovePoints;
            MaxMovementPointModifier.Value = 0;

            CurrentActionPoints.Value = _startActionPoints - _defaultActionPointChange;
            ActionPointChange.Value = _defaultActionPointChange;
            ActionPointChangeModifier.Value = 0;
            MaxActionPoints.Value = _maxActionPoints;
        }
        public void ApplyRoundStart()
        {
            CurrentMovementPoints.Value = MaxMovementPoints.Value + MaxMovementPoints.Value;
            CurrentActionPoints.Value = Mathf.Clamp(CurrentActionPoints.Value + ActionPointChange.Value + ActionPointChangeModifier.Value, 0, MaxActionPoints.Value);
        }
    }
}