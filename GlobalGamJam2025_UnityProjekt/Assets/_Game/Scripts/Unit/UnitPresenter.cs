using Game.Grid;
using Game.Grid.Content;
using System.Collections.Generic;
using UnityEngine;
using static Game.Unit.UnitModel;

namespace Game.Unit
{
    [RequireComponent(typeof(UnitModel)), RequireComponent(typeof(IMovementProvider)), RequireComponent(typeof(IAbilityProvider))]
    public class UnitPresenter : MonoBehaviour
    {
        private UnitModel model;
        private AUnitView view;
        private IMovementProvider movementProvider;
        private IAbilityProvider abilityProvider;

        private Vector3 velocity;
        private void Awake()
        {
            model = GetComponent<UnitModel>();
            view = GetComponent<AUnitView>();

            movementProvider = GetComponent<IMovementProvider>();
            abilityProvider = GetComponent<IAbilityProvider>();
        }
        private void Start()
        {
            transform.parent = GridPresenter.Instance.transform;
        }
        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(model.Position.Value.x, 0, model.Position.Value.y), ref velocity, 0.25f);
        }
        private void OnEnable()
        {
            model.InitValues();

            model.MaxHP.OnChange += view.MaxHPChanged;
            model.CurrentHP.OnChange += view.CurrentHPChanged;
            model.Position.OnChange += view.PositionChanged;
            model.Rotation.OnChange += view.UnitRotationChanged;
            model.MaxMovementPoints.OnChange += view.MaxMovementPointsChanged;
            model.CurrentMovementPoints.OnChange += view.CurrentMovementPointsChanged;
            model.MaxMovementPointModifier.OnChange += view.MaxMovementPointModifierChanged;
            model.CurrentActionPoints.OnChange += view.CurrentActionPointsChanged;
            model.ActionPointChange.OnChange += view.ActionPointChangeChanged;
            model.ActionPointChangeModifier.OnChange += view.ActionPointChangeModifierChanged;
            model.MaxActionPoints.OnChange += view.MaxActionPointsChanged;
            model.IsUsedThisRound.OnChange += view.IsUsedThisRoundChanged;
            model.Initiative.OnChange += view.InitiativeChanged;
            model.UnitFaction.OnChange += view.UnitFactionChanged;

            model.MaxHP.ForceInvoke();
            model.CurrentHP.ForceInvoke();
            model.Position.ForceInvoke();
            model.Rotation.ForceInvoke();
            model.MaxMovementPoints.ForceInvoke();
            model.CurrentMovementPoints.ForceInvoke();
            model.MaxMovementPointModifier.ForceInvoke();
            model.CurrentActionPoints.ForceInvoke();
            model.ActionPointChange.ForceInvoke();
            model.ActionPointChangeModifier.ForceInvoke();
            model.MaxActionPoints.ForceInvoke();
            model.IsUsedThisRound.ForceInvoke();
            model.Initiative.ForceInvoke();
            model.UnitFaction.ForceInvoke();
        }

        public void ApplyOverallRoundStart()
        {
            model.IsUsedThisRound.Value = false;
        }
        public void ApplyIndividualRoundStart()
        {
            model.ApplyRoundStart();
        }
        public void ApplyIndividualRoundFinished()
        {
            model.IsUsedThisRound.Value = true;
        }
        public HashSet<Vector2Int> GetMovementOptions()
        {
            return movementProvider.GetMovementPossibilites(model.Position.Value);
        }
        public List<AAbility> GetAbilityOptions()
        {
            return abilityProvider.GetAbilityPossibilities(model.Position.Value);
        }

        public int GetAbilityPoints()
        {
            return model.CurrentActionPoints.Value;
        }
        public int GetCurrentMovementPoints()
        {
            return model.CurrentMovementPoints.Value;
        }
        public void ApplyCurrentMovementPointChange(int value)
        {
            model.CurrentMovementPoints.Value += value;
        }
        public void ApplyCurrentActionPointChange(int value)
        {
            model.CurrentActionPoints.Value += value;
        }

        public Vector2Int GetPosition()
        {
            return model.Position.Value;
        }
        public UnitRotation GetRotation()
        {
            return model.Rotation.Value;
        }
        public void SetPosition(Vector2Int position)
        {
            Vector2Int dif = model.Position.Value - position;
            if(Mathf.Abs(dif.x) >= Mathf.Abs(dif.y))
            {
                model.Rotation.Value = dif.x > 0 ? UnitRotation.Right : UnitRotation.Left;
            }
            else
            {
                model.Rotation.Value = dif.y > 0 ? UnitRotation.Up : UnitRotation.Down;
            }
            model.Position.Value = position;
        }
        public int GetInitiative()
        {
            return model.Initiative.Value;
        }

        public void SetMaxMovementPointModifier(int value)
        {
            model.MaxMovementPointModifier.Value = value;
        }
        public void SetActionPointChangeModifier(int value)
        {
            model.ActionPointChangeModifier.Value = value;
        }
        public void ApplyHPChange(int value)
        {
            model.CurrentHP.Value += value;
        }
        public Faction GetFaction()
        {
            return model.UnitFaction.Value;
        }
        public void DestroyUnit()
        {
            GridPresenter.Instance.DestroyUnit(model.Position.Value);
            GameObject.Destroy(transform.parent.gameObject);
        }
    }
}
