using Game.Community;
using Game.Unit;

namespace AI.Actions
{
    public class DefendAction : AAIAction
    {
        public CommunityPresenter target;

        public DefendAction(CommunityPresenter target)
        {
            this.target = target;
        }

        public override float GetAgentBias(UnitPresenter caster)
        {
            throw new System.NotImplementedException();
        }

        public override float GetSituationalBias(AIDirector director)
        {
            throw new System.NotImplementedException();
        }

        public override void Perform(UnitPresenter unitPresenter)
        {
            throw new System.NotImplementedException();
        }
    }
}