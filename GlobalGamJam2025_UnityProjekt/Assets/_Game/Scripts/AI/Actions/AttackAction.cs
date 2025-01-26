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

                if (!path.Any()) 
                {
                    Vector2Int[] directions = new Vector2Int[] {Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down};
                    foreach(AAbility ability in abilities)
                    {
                        foreach(Vector2Int direction in directions)
                        {
                            if (ability.IsTargetConditionSatisfied(caster.GetPosition() + direction, direction))
                            {
                                GamePresenter.Instance.GridClickedHandler(caster.GetPosition() + direction);
                                return;
                            }
                        }
                    }
                }
                if (caster.GetMovementOptions().Any(m => path.Contains(m)))
                {
                    Vector2Int movement = caster.GetMovementOptions().First(m => path.Contains(m));
                    GamePresenter.Instance.GridClickedHandler(movement);
                    return;
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

            Vector2Int[] directions = new Vector2Int[] {
                Vector2Int.up, Vector2Int.right, Vector2Int.down,Vector2Int.left
            };

            HashSet<Vector2Int> output = new();
            foreach(AAbility ability in abilities)
            {
               for(int x = target.GetPosition().x - 5; x < target.GetPosition().x + 5; x++)
                {
                    for (int z = target.GetPosition().y - 5; z < target.GetPosition().y + 5; z++)
                    {
                        foreach(Vector2Int dir in directions)
                        {
                            Vector2Int pos = new Vector2Int(x, z);
                            if (!GridPresenter.Instance.IsWalkable(pos))
                            {
                                continue;
                            }
                            if (ability.IsTargetConditionSatisfied(pos, dir))
                            {
                                output.Add(pos - dir);
                                if (!GridPresenter.Instance.IsWalkable(pos - dir))
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            return output.ToList();
        }
        private float CalculateCost(List<Vector2Int> targetPositions, UnitPresenter caster)
        {
            return AStarHelper.CalculatePath(targetPositions, caster.GetPosition(), GridPresenter.Instance.IsWalkable).Item2;            
        }
    }
}