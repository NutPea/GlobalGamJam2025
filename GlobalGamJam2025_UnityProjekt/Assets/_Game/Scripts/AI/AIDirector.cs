using AI;
using AI.Actions;
using Game.Community;
using Game.Grid;
using Game.Unit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game.Unit.UnitModel;

public class AIDirector : MonoBehaviour
{
    [SerializeField] private Faction faction;
    [Space]
    [SerializeField, Range(0, 1)] private float attackBias;
    [SerializeField, Range(0, 1)] private float defendBias;
    [SerializeField, Range(0, 1)] private float captureBias;
    [SerializeField, Range(0, 1)] private float chaosBias;    
    [Space]
    [SerializeField, Range(0, 1)] private float agentPriorityBias;


    public static Dictionary<Faction, AIDirector> Instances = new();
    private PriorityList<AAIAction, float> priorityList = new();


    private void OnEnable()
    {
        Instances[faction] = this;
    }
    public void RoundStart()
    {
        List<AAIAction> actions = GetPossibleActions();
        AssignBaseBias(actions);
        AssignSituationalWeights(actions);
        actions.ForEach(a => priorityList.Add(a, a.totalPriority));
    }

    public PriorityList<AAIAction, float> GetPriorityList()
    {
        return priorityList;
    }
    public Faction GetFaction() 
    {
        return faction;
    }

    private List<AAIAction> GetPossibleActions()
    {
        List<AAIAction> attackActions = new List<AAIAction>();

        attackActions.Add(new ChaosAction());

        foreach (UnitPresenter item in GridPresenter.Instance.GetAll<UnitPresenter>().Where(u => u.GetFaction() == Faction.Vegans))
        {
            attackActions.Add(new AttackAction(item));
        }

        foreach (CommunityPresenter item in GridPresenter.Instance.GetAll<CommunityPresenter>().Where(u => u.GetFaction() != faction))
        {
            attackActions.Add(new CaptureAction(item));
        }

        foreach (CommunityPresenter item in GridPresenter.Instance.GetAll<CommunityPresenter>().Where(u => u.GetFaction() == faction))
        {
            attackActions.Add(new DefendAction(item));
        }
        return new List<AAIAction>();
    }
    private void AssignBaseBias(List<AAIAction> actions)
    {
        foreach(AAIAction action in actions)
        {
            action.assignedPriorityFactor = agentPriorityBias;
            switch (action)
            {
                case AttackAction attackAction:
                    attackAction.directorPriority = attackBias;
                    break;
                case DefendAction defendAction:
                    defendAction.directorPriority = defendBias;
                    break;
                case CaptureAction captureAction:
                    captureAction.directorPriority = captureBias;
                    break;
                case ChaosAction chaosAction:
                    chaosAction.directorPriority = chaosBias;
                    break;
            }
        }
    }
    private void AssignSituationalWeights(List<AAIAction> actions)
    {
        foreach (AAIAction action in actions)
        {
            action.directorPriority *= action.GetSituationalBias(this);           
        }
    }
}