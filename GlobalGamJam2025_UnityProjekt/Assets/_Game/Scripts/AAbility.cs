using Game.Grid;
using Game.Grid.Content;
using GetraenkeBub;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class AAbility : MonoBehaviour
    {
        public string abilityName;
        public int actionPointCost;
        public int length;
        public ActionDirection actionDirection;
        public TargetType targetType;
        public GameObject spawnedEffect;
        [Header("AttackDamages")]
        public int attackDamage;
        public int attackActionPoints;
        public int attackReduceMovementPoints;


        //add configuration possibilty for effect /
        //add definition for AOE area /
        //add configuration possibility for targetType /

        /// <summary>
        /// ignoriert Kosten -> überprüft ob im Grid Target im AOE Bereich verfügbar ist
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTargetConditionSatisfied()
        {
            Vector2Int parentPos = GetComponentInParent<Unit.UnitPresenter>().GetPosition();

            Vector2Int direction;

            direction = EvaluateDirectionFromEnum();

            switch (actionDirection)
            {
                case ActionDirection.forward:
                    return EvaluateStraightDirection(parentPos, length, direction);
                case ActionDirection.circle:
                    return EvaluateDirectionCircle(parentPos);
                case ActionDirection.O:
                    return EvaluateO(parentPos);
                case ActionDirection.L:
                    return EvaluateL(parentPos, direction);
                case ActionDirection.T:
                    return EvaluateT(parentPos, direction);
                case ActionDirection.X:
                    return EvaluateX(parentPos, length);
                case ActionDirection.cross:
                    return EvaluateCross(parentPos, direction);
                case ActionDirection.middleFinger:
                    return EvaluateMiddleFinger(parentPos, direction);
            }

            return true;
        }

        private bool EvaluateMiddleFinger(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private bool EvaluateCross(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private bool EvaluateX(Vector2Int parentPos, int length)
        {
            throw new NotImplementedException();
        }

        private bool EvaluateT(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private bool EvaluateL(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private bool EvaluateO(Vector2Int parentPos)
        {
            throw new NotImplementedException();
        }

        private Vector2Int EvaluateDirectionFromEnum()
        {
            switch (GetComponentInParent<Unit.UnitPresenter>().GetRotation())
            {
                case Unit.UnitModel.UnitRotation.Up:
                    return Vector2Int.up;
                case Unit.UnitModel.UnitRotation.Down:
                    return Vector2Int.down;
                case Unit.UnitModel.UnitRotation.Right:
                    return Vector2Int.right;
                case Unit.UnitModel.UnitRotation.Left:
                    return Vector2Int.left;
                default:
                    return Vector2Int.left;
            }
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
                        if (GridPresenter.Instance.GetContent(parentPos + toCheckPos).GetType() == GetContentType(targetType))
                        {
                            return true;
                        } 
                    }
                }
            }

            return false;
        }

        private Type GetContentType(TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.Blocker:
                    return typeof(Grid.Content.BlockerContent);
                case TargetType.Unit:
                    return typeof(Grid.Content.UnitContent);
                case TargetType.Empty:
                    return typeof(Grid.Content.EmptyContent);
                case TargetType.Community:
                    return typeof(Grid.Content.CommunityContent);

            }
            return typeof(Grid.Content.BlockerContent);
        }

        private bool EvaluateStraightDirection(Vector2Int parentPos, int length, Vector2Int direction)
        {
            for (int i = 1; i <= length; i++)
            {
                if (GridPresenter.Instance.GetContent(parentPos + direction * i).GetType() == GetContentType(targetType))
                {
                    return true;
                }

            }
            return false;
        }

        List<AGridContent> GetTargets()
        {
            return new List<AGridContent>();
        }
        public virtual void Cast(Action callbackCastFinished)
        {
            Instantiate(spawnedEffect, transform.parent.transform);
            foreach (var target in GetTargets())
            {
                switch (target)
                {
                    case UnitContent unit:
                        break;
                    case CommunityContent community: 
                        break;

                }
            }
            if(UIStateManager.Instance == null)
            {
                callbackCastFinished();
            } else {
                UIStateManager.Instance.HandleAbility(()=>callbackCastFinished());
            }
        }
    }
}