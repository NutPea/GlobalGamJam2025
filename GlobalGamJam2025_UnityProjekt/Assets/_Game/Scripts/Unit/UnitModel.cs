using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Game.Unit
{
    public class UnitModel : MonoBehaviour
    {
        public enum Faction { None, Vegans, Bavarians, Tinfoil}
        public enum UnitRotation { Up, Right, Down, Left }

        [SerializeField, Tooltip("maximale movement aktionen pro Runde")] private int _maxMovePoints;

        [SerializeField, Tooltip("Actionpoints in der ersten Runde BEVOR defaultActionPointChange applied wurde")] private int _startActionPoints;
        [SerializeField, Tooltip("Wird am Rundenbeginn auf die aktuellen ActionPoints drauf gerechnet, kann durch Fähigkeiten temporär beinflusst werden")] private int _defaultActionPointChange;
        [SerializeField, Tooltip("Maximale Action points der Unit")] private int _maxActionPoints;

        [SerializeField, Tooltip("Maximale und StartHP der Unit")] private int _maxHP;
        [SerializeField, Tooltip("Initiative, um Reihenfolge der Units zu definieren")] private int _initiative;
        [SerializeField, Tooltip("Start Faction der Unit")] private Faction _faction;


        //HP
        [HideInInspector] public ModelEntry<int> MaxHP = new();
        [HideInInspector] public ModelEntry<int> CurrentHP = new();

        //MOVEMENT
        [HideInInspector] public ModelEntry<Vector2Int> Position = new();
        [HideInInspector] public ModelEntry<UnitRotation> Rotation = new();

        [HideInInspector] public ModelEntry<int> MaxMovementPoints = new();
        [HideInInspector] public ModelEntry<int> CurrentMovementPoints = new();
        [HideInInspector] public ModelEntry<int> MaxMovementPointModifier = new();

        //ACTION POINTS
        [HideInInspector] public ModelEntry<int> CurrentActionPoints = new();
        [HideInInspector] public ModelEntry<int> ActionPointChange = new();
        [HideInInspector] public ModelEntry<int> ActionPointChangeModifier = new();
        [HideInInspector] public ModelEntry<int> MaxActionPoints = new();


        [HideInInspector] public ModelEntry<bool> IsUsedThisRound = new();
        [HideInInspector] public ModelEntry<int> Initiative = new();
        [HideInInspector] public ModelEntry<Faction> UnitFaction = new();


        public void InitValues()
        {
            CurrentHP.SetSanityFixFunc(f => Mathf.Clamp(f, 0, MaxHP.Value));

            MaxHP.Value = _maxHP;
            CurrentHP.Value = _maxHP;

            MaxMovementPoints.Value = _maxMovePoints;
            CurrentMovementPoints.Value = _maxMovePoints;
            MaxMovementPointModifier.Value = 0;

            CurrentActionPoints.Value = _startActionPoints - _defaultActionPointChange;
            ActionPointChange.Value = _defaultActionPointChange;
            ActionPointChangeModifier.Value = 0;
            MaxActionPoints.Value = _maxActionPoints;

            Initiative.Value = _initiative;
            UnitFaction.Value = _faction;
        }
        public void ApplyRoundStart()
        {
            CurrentMovementPoints.Value = MaxMovementPoints.Value + MaxMovementPointModifier.Value;
            CurrentActionPoints.Value = Mathf.Clamp(CurrentActionPoints.Value + ActionPointChange.Value + ActionPointChangeModifier.Value, 0, MaxActionPoints.Value);

            MaxMovementPointModifier.Value = 0;
            ActionPointChange.Value = 0;
        }
    }
}