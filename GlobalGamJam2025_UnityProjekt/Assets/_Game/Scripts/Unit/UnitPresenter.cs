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
        private UnitContent parentContent;

        private Vector3 velocity;
        private void Awake()
        {
            model = GetComponent<UnitModel>();
            view = GetComponent<AUnitView>();

            movementProvider = GetComponent<IMovementProvider>();
            abilityProvider = GetComponent<IAbilityProvider>();
            transform.parent = GridPresenter.Instance.transform;
        }
        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(model.Position.Value.x, 0, model.Position.Value.y), ref velocity, 0.25f);
        }
        private void OnEnable()
        {
            model.InitValues();

            //TODO SUBSCRIBE VIEW
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
    }
}
