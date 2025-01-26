using AI.Actions;
using Game.Unit;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(UnitPresenter))]
    public class AIAgent : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float attackBias;
        [SerializeField, Range(0, 1)] private float defendBias;
        [SerializeField, Range(0, 1)] private float captureBias;
        [SerializeField, Range(0, 1)] private float chaosBias;

        private UnitPresenter unitPresenter;
        private void OnEnable()
        {
            unitPresenter = GetComponent<UnitPresenter>();
        }

        public void DoNextAction()
        {
            AIDirector myDirector = AIDirector.Instances[unitPresenter.GetFaction()];
            PriorityList<AAIAction, float> actions = myDirector.GetPriorityList();

            AssignOwnBaseBias(actions);
            MultiplyOwnSatisfactionBias(actions);

            actions.UpdateSorting();
            AAIAction action = actions.First.item;
            action.assignedCount++;
            action.Perform(unitPresenter);
        }

        private void AssignOwnBaseBias(PriorityList<AAIAction, float> actions)
        {
            foreach (var action in actions.Items)
            {
                switch (action.item)
                {
                    case AttackAction attackAction:
                        attackAction.agentPriority = attackBias;
                        break;
                    case DefendAction defendAction:
                        defendAction.agentPriority = defendBias;
                        break;
                    case CaptureAction captureAction:
                        captureAction.agentPriority = captureBias;
                        break;
                    case ChaosAction chaosAction:
                        chaosAction.agentPriority = chaosBias;
                        break;
                }
            }
        }
        private void MultiplyOwnSatisfactionBias(PriorityList<AAIAction, float> actions)
        {
            foreach (var action in actions.Items)
            {
                action.item.agentPriority *= action.item.GetAgentBias(unitPresenter);
            }
        }     
    }
}