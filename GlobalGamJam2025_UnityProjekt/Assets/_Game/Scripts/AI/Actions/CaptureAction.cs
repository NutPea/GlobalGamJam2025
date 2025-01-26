using Game.Community;
using Game.Unit;

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