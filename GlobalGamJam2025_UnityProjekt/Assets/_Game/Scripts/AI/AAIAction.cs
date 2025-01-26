using Game.Unit;

namespace AI
{
    public abstract class AAIAction
    {
        public float directorPriority;
        public float agentPriority;
        public float agentPriorityFactor;

        public float assignedCount;
        public float assignedPriorityFactor;

        public float totalPriority { get => directorPriority + agentPriority * agentPriorityFactor + assignedCount * assignedPriorityFactor; }

        public void ResetPerAgent()
        {
            agentPriority = 0;
        }

        public abstract float GetSituationalBias(AIDirector director);
        public abstract float GetAgentBias(UnitPresenter caster);
        public abstract void Perform(UnitPresenter unitPresenter);
    }
}