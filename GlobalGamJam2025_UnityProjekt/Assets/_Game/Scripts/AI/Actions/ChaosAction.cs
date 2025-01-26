using Game;
using Game.Unit;
using System.Collections.Generic;
using System.Linq;

namespace AI.Actions
{
    public class ChaosAction : AAIAction
    {
        public override float GetAgentBias(UnitPresenter caster)
        {
            return 0;
        }

        public override float GetSituationalBias(AIDirector director)
        {
            return 0;
        }

        public override void Perform(UnitPresenter caster)
        {
            List<AAbility> abilities = caster.GetAbilityOptions();
            List<AAbility> skipAbilities = abilities.Where(a => a.actionDirection == ActionDirection.skip).ToList();
            GamePresenter.Instance.AbilityCastedHandler(skipAbilities.First());
        }
    }
}