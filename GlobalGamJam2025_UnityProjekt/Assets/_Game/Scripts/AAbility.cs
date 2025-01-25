using Game.Grid;
using Game.Grid.Content;
using GetraenkeBub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public abstract class AAbility : MonoBehaviour
    {
        public Sprite abilityIcon;
        public string abilityName;
        public string abilityDescription;

        public int actionPointCost;
        public int length;
        public ActionDirection actionDirection;
        public TargetType targetType;
        public GameObject spawnedEffect;
        [Header("AttackDamages")]
        public int attackDamage;
        public int attackActionPoints;
        public int attackReduceMovementPoints;

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

        private List<Vector2Int> RotatePattern(List<Vector2Int> pattern, Vector2Int direction)
        {

            return new List<Vector2Int>();
        }

        private bool EvaluateCross(Vector2Int parentPos, Vector2Int direction)
        {

            //Todo Check two cross fields
            if (GridPresenter.Instance.GetContent(parentPos + direction * 3 + new Vector2Int(0,1)).GetType() == GetContentType(targetType))
            {
                return true;
            }
            if (GridPresenter.Instance.GetContent(parentPos + direction * 3 + new Vector2Int(1, 0)).GetType() == GetContentType(targetType))
            {
                return true;
            }
            return EvaluateStraightDirection(parentPos, 4, direction);
        }

        private bool EvaluateX(Vector2Int parentPos, int length)
        {
            throw new NotImplementedException();
        }

        private bool EvaluateT(Vector2Int parentPos, Vector2Int direction)
        {
            List<Vector2Int> positions = new List<Vector2Int> { Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up * 4, Vector2Int.up * 4 + Vector2Int.left, Vector2Int.up * 4 + Vector2Int.right };

            List<Vector2Int> rotatedPositions = positions.Select(pos =>
            {
                if (direction == Vector2Int.up)    // (0,  1)
                    return pos;
                else if (direction == Vector2Int.right) // (1,  0) -> 90° clockwise
                    return new Vector2Int(pos.y, -pos.x);
                else if (direction == Vector2Int.down)  // (0, -1) -> 180°
                    return new Vector2Int(-pos.x, -pos.y);
                else if (direction == Vector2Int.left)  // (-1, 0) -> 270° clockwise
                    return new Vector2Int(-pos.y, pos.x);
                else
                    return pos; // Fallback for unexpected directions
            }).ToList();

            //TODO auf rotatedPositions parentPos drauf addieren
            //TODO checken ob target Bedingung erfüllt und dann returnen
            //Liste speichern und dann highlights über Grid visualisieren und später weg machen wieder
            return true; 
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

        private HashSet<AGridContent> GetTargetsDirectionCircle(Vector2Int parentPos)
        {
            HashSet<AGridContent> targets = new HashSet<AGridContent>();
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    Vector2Int toCheckPos = new Vector2Int(x, y);
                    if (toCheckPos.magnitude <= length)
                    {
                        AGridContent content = GridPresenter.Instance.GetContent(parentPos + toCheckPos);
                        if (content.GetType() == GetContentType(targetType))
                        {
                            targets.Add(content);
                        }
                    }
                }
            }

            return targets;
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

        private HashSet<AGridContent> GetTargetsStraightDirection(Vector2Int parentPos, int length, Vector2Int direction)
        {
            HashSet<AGridContent> targets = new HashSet<AGridContent>();
            for (int i = 1; i <= length; i++)
            {
                AGridContent content = GridPresenter.Instance.GetContent(parentPos + direction * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    targets.Add(content);
                }
            }
            return targets;
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

        HashSet<AGridContent> GetTargets()
        {
            Vector2Int parentPos = GetComponentInParent<Unit.UnitPresenter>().GetPosition();

            Vector2Int direction;

            direction = EvaluateDirectionFromEnum();
            HashSet<AGridContent> targets = new HashSet<AGridContent>();

            switch (actionDirection)
            {
                case ActionDirection.forward:
                    targets.AddRange(GetTargetsStraightDirection(parentPos, length, direction));
                    break;
                case ActionDirection.circle:
                    targets.AddRange( GetTargetsDirectionCircle(parentPos));
                    break;
                case ActionDirection.O:
                    targets.AddRange( GetTargetsO(parentPos));
                    break;
                case ActionDirection.L:
                    targets.AddRange(GetTargetsL(parentPos, direction));
                    break;
                case ActionDirection.T:
                    targets.AddRange(GetTargetsT(parentPos, direction));
                    break;
                case ActionDirection.X:
                    targets.AddRange(GetTargetsX(parentPos, length));
                    break;
                case ActionDirection.cross:
                    targets.AddRange(GetTargetsCross(parentPos, direction));
                    break;
                case ActionDirection.middleFinger:
                    targets.AddRange(GetTargetsMiddleFinger(parentPos, direction));
                    break;
            }
            return targets;
        }

        private IEnumerable<AGridContent> GetTargetsMiddleFinger(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<AGridContent> GetTargetsCross(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<AGridContent> GetTargetsX(Vector2Int parentPos, int length)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<AGridContent> GetTargetsT(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<AGridContent> GetTargetsL(Vector2Int parentPos, Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        private HashSet<AGridContent> GetTargetsO(Vector2Int parentPos)
        {
            throw new NotImplementedException();
        }

        public virtual void Cast(Action callbackCastFinished)
        {
            Instantiate(spawnedEffect, transform.parent.transform);
            foreach (var target in GetTargets())
            {
                switch (target)
                {
                    case UnitContent unit:
                        unit.unitReference.ApplyHPChange(-attackDamage);
                        unit.unitReference.SetActionPointChangeModifier(-attackActionPoints);
                        unit.unitReference.SetMaxMovementPointModifier(-attackReduceMovementPoints);
                        //TODO Animationen!
                        break;
                    case CommunityContent community:
                        if(community.communityPresenter.GetFaction() == Unit.UnitModel.Faction.None)
                        {
                            if (community.communityPresenter.IsCaptureSuccessful())
                            {
                                //TODDO: Success Animations
                                community.communityPresenter.SetFaction(GetComponentInParent<Unit.UnitPresenter>().GetFaction());
                            }
                        } else
                        {
                            if (community.communityPresenter.IsCaptureSuccessful())
                            {
                                //TODO: Success Animations
                                community.communityPresenter.SetFaction(GetComponentInParent<Unit.UnitPresenter>().GetFaction());
                            } else
                            {
                                //TODO: Failure Animation
                            }
                        }
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