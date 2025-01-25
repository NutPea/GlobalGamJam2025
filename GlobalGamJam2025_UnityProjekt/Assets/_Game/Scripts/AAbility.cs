using Game.Grid;
using System;
using UnityEngine;

namespace Game
{
    public abstract class AAbility : MonoBehaviour
    {
        public int actionPointCost;
        public int length;
        public ActionDirection actionDirection;
        public AGridContent targetType;

        //add configuration possibilty for effect
        //add definition for AOE area /
        //add configuration possibility for targetType /

        /// <summary>
        /// ignoriert Kosten -> überprüft ob im Grid Target im AOE Bereich verfügbar ist
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTargetConditionSatisfied()
        {
            Vector2Int parentPos = GetComponentInParent<Unit.UnitPresenter>().GetPosition() ;
            switch (actionDirection)
            {
                case ActionDirection.forward:
                    return EvaluateStraightDirection(parentPos, length, Vector2Int.up); // TODO: Change to Unit Forward
                case ActionDirection.circle:
                    return EvaluateDirectionCircle(parentPos);  
            }
            
            return true;
        }

        private bool EvaluateDirectionCircle(Vector2Int parentPos)
        {
            for (int x = 0; x < length; x++)
            {
                for(int y = 0; y < length; y++)
                {
                    Vector2Int toCheckPos = new Vector2Int(x, y);
                    if(toCheckPos.magnitude <= length)
                    {
                        if (GridPresenter.Instance.GetContent(parentPos + toCheckPos) == targetType)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }



        private bool EvaluateStraightDirection(Vector2Int parentPos, int length, Vector2Int direction)
        {
            for (int i = 1; i <= length; i++)
            {
                if (GridPresenter.Instance.GetContent(parentPos + direction * i) == targetType)
                {
                    return true;
                }

            }
            return false;
        }
        public virtual void Cast(Action callbackCastFinished)
        {
            //TODO hier was überlegen für Rückgabe

            callbackCastFinished();
        }
    }
}