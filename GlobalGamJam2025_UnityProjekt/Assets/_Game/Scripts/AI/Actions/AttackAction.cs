using Game;
using Game.Grid;
using Game.Unit;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Actions
{
    public class AttackAction : AAIAction
    {
        public UnitPresenter target;

        public AttackAction(UnitPresenter target)
        {
            this.target = target;
        }

        public override float GetAgentBias(UnitPresenter caster)
        {
            List<Vector2Int> targetPositions = GetTargetPositions(caster);
            float cost = CalculateCost(targetPositions, caster);
            return Mathf.InverseLerp(25, 0, cost);
        }

        public override float GetSituationalBias(AIDirector director)
        {
            List<UnitPresenter> allUnits = GridPresenter.Instance.GetAll<UnitPresenter>();
            int ownUnits = allUnits.Where(u => u.GetFaction() == director.GetFaction()).Count();
            int enemies = allUnits.Where(u => u.GetFaction() != director.GetFaction()).Count();
            float percentage = ownUnits / enemies;

            return Mathf.InverseLerp(0.5f, 2, percentage);
        }

        public override void Perform(UnitPresenter caster)
        {
            List<AAbility> abilities = caster.GetAbilityOptions();
            List<AAbility> skipAbilities = abilities.Where(a => a.actionDirection == ActionDirection.skip).ToList();
            List<AAbility> captureAbilities = abilities.Where(a => a.targetType == Game.TargetType.Community).ToList();
            abilities.RemoveAll(a => skipAbilities.Contains(a));
            abilities.RemoveAll(a => captureAbilities.Contains(a));

            List<AAbility> castableAbilities = abilities.Where(a => a.IsTargetConditionSatisfied() && a.actionPointCost <= caster.GetAbilityPoints()).ToList();
            if (castableAbilities.Any())
            {
                GamePresenter.Instance.AbilityCastedHandler(castableAbilities[Random.Range(0, castableAbilities.Count)]);
                return;
            }
            if (caster.GetCurrentMovementPoints() > 0)
            {
                List<Vector2Int> targetPositions = GetTargetPositions(caster);
                List<Vector2Int> path = AStarHelper.CalculatePath(targetPositions, caster.GetPosition(), GridPresenter.Instance.IsWalkable).Item1;

                if (caster.GetMovementOptions().Any(m => path.Contains(m)))
                {
                    Vector2Int movement = caster.GetMovementOptions().First(m => path.Contains(m));
                    GamePresenter.Instance.GridClickedHandler(movement);
                }
            }
            GamePresenter.Instance.AbilityCastedHandler(skipAbilities.First());
        }

        private List<Vector2Int> GetTargetPositions(UnitPresenter caster)
        {
            List<AAbility> abilities = caster.GetAbilityOptions();
            List<AAbility> skipAbilities = abilities.Where(a => a.actionDirection == ActionDirection.skip).ToList();
            List<AAbility> captureAbilities = abilities.Where(a => a.targetType == Game.TargetType.Community).ToList();
            abilities.RemoveAll(a => skipAbilities.Contains(a));
            abilities.RemoveAll(a => captureAbilities.Contains(a));

            return new List<Vector2Int>();
        }
        private float CalculateCost(List<Vector2Int> targetPositions, UnitPresenter caster)
        {
            return AStarHelper.CalculatePath(targetPositions, caster.GetPosition(), GridPresenter.Instance.IsWalkable).Item2;            
        }
    }
}