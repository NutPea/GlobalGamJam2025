using AI;
using AI.Actions;
using System.Collections.Generic;
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


    private List<AAIAction> GetPossibleActions()
    {
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
            action.directorPriority *= action.GetSituationalBias();           
        }
    }
}

/*
 TODO IN DIESER DATEI:
    - GetPossibleActions
 */
