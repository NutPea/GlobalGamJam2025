using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    [RequireComponent(typeof(UnitModel)), RequireComponent(typeof(IMovementProvider)), RequireComponent(typeof(IAbilityProvider))]
    public class UnitPresenter : MonoBehaviour
    {
        private UnitModel model;
        private IMovementProvider movementProvider;
        private IAbilityProvider abilityProvider;

        private void Awake()
        {
            model = GetComponent<UnitModel>();
            movementProvider = GetComponent<IMovementProvider>();
            abilityProvider = GetComponent<IAbilityProvider>();
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

        public void SetMaxMovementPointModifier(int value)
        {
            model.MaxMovementPointModifier.Value = value;
        }
        public void SetActionPointChangeModifier(int value)
        {
            model.ActionPointChangeModifier.Value = value;
        }
    }
}