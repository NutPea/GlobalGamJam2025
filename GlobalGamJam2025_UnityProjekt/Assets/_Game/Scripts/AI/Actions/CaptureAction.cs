using Game;
using Game.Community;
using Game.Unit;
using System.Collections.Generic;
using System.Linq;

namespace AI.Actions
{
    public class CaptureAction : AAIAction
    {
        public CommunityPresenter target;

        public CaptureAction(CommunityPresenter target)
        {
            this.target = target;
        }

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