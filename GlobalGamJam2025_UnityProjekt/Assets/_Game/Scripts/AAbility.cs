using Game.Grid;
using Game.Grid.Content;
using GetraenkeBub;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Game
{
    public class AAbility : MonoBehaviour
    {
        public Sprite abilityIcon;
        public string abilityName;
        public string AbilityName=>abilityName;
        public string abilityDescription;
        public string AbilityDescription => abilityDescription;

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
        /// 
        private List<Vector2Int> patternT = new List<Vector2Int> { Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up * 4, Vector2Int.up * 4 + Vector2Int.left, Vector2Int.up * 4 + Vector2Int.right };
        private List<Vector2Int> patternL = new List<Vector2Int> { Vector2Int.up , Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up *3 + Vector2Int.left };
        private List<Vector2Int> patternCross = new List<Vector2Int> { Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up * 4, Vector2Int.up * 3 + Vector2Int.left, Vector2Int.up * 3 + Vector2Int.right };
        private List<Vector2Int> patternMiddleFinger = new List<Vector2Int>();
        private List<Vector2Int> patternO = new List<Vector2Int>();

        private void OnEnable()
        {
            UIStateManager.Instance.OnAbilityHighlight += GetOnAbilityHighlight;
            UIStateManager.Instance.OnAbilityStop += OnAbilityStop;
        }

        private void OnAbilityStop()
        {
            DisableHighlights();
        }

        private void OnDisable()
        {
            UIStateManager.Instance.OnAbilityHighlight -= GetOnAbilityHighlight;
            UIStateManager.Instance.OnAbilityStop -= OnAbilityStop;
        }

        private void GetOnAbilityHighlight(AAbility ability)
        {
            ability.ActivateHighlight();
        }

        public void ActivateHighlight()
        {
            Vector2Int parentPos = GetComponentInParent<Unit.UnitPresenter>().GetPosition();

            Vector2Int direction;

            direction = EvaluateDirectionFromEnum();

            switch (actionDirection)
            {
                case ActionDirection.forward:
                    HighLightStraightDirection(parentPos, length, direction);
                    break;
                case ActionDirection.circle:
                    HighLightDirectionCircle(parentPos);
                    break;
                case ActionDirection.O:
                    HighLightPattern(parentPos, Vector2Int.up, patternO);
                    break;
                case ActionDirection.L:
                    HighLightPattern(parentPos, direction, patternL);
                    break;
                case ActionDirection.T:
                    HighLightPattern(parentPos, direction, patternT);
                    break;
                case ActionDirection.X:
                    HighLightX(parentPos, length);
                    break;
                case ActionDirection.cross:
                    HighLightPattern(parentPos, direction, patternCross);
                    break;
                case ActionDirection.middleFinger:
                    HighLightPattern(parentPos, direction, patternMiddleFinger);
                    break;
            }
        }

        private void HighLightX(Vector2Int parentPos, int length)
        {
            for (int i = 0; i > length; i++)
            {
                AGridContent content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.left * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                }

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.left * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                }

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.right * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                }

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.right * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                }
            }
        }

        private void HighLightDirectionCircle(Vector2Int parentPos)
        {
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
                            content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                        }
                    }
                }
            }
        }

        private void HighLightStraightDirection(Vector2Int parentPos, int length, Vector2Int direction)
        {
            for (int i = 1; i <= length; i++)
            {
                AGridContent content = GridPresenter.Instance.GetContent(parentPos + direction * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                }
            }
        }

        private void HighLightPattern(Vector2Int parentPos, Vector2Int direction, List<Vector2Int> pattern)
        {
            List<Vector2Int> rotatedList = RotatePattern(pattern, direction);

            foreach (Vector2Int pos in rotatedList)
            {
                AGridContent content = GridPresenter.Instance.GetContent(pos + parentPos);
                if (content.GetType() == GetContentType(targetType))
                {
                    content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                }
            }
        }

        public bool IsTargetConditionSatisfied()
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
                    return EvaluatePattern(parentPos, Vector2Int.up, patternO);
                case ActionDirection.L:
                    return EvaluatePattern(parentPos, direction, patternL);
                case ActionDirection.T:
                    return EvaluatePattern(parentPos, direction, patternT);
                case ActionDirection.X:
                    return EvaluateX(parentPos, length);
                case ActionDirection.cross:
                    return EvaluatePattern(parentPos, direction, patternCross);
                case ActionDirection.middleFinger:
                    return EvaluatePattern(parentPos, direction, patternMiddleFinger);
            }

            return true;
        }

        private bool EvaluateX(Vector2Int parentPos, int length)
        {
            for ( int i = 0; i > length; i++)
            {
                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.left * i).GetType() == GetContentType(targetType))
                {
                    return true;
                }

                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.left * i).GetType() == GetContentType(targetType))
                {
                    return true;
                }

                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.right * i).GetType() == GetContentType(targetType))
                {
                    return true;
                }

                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.right * i).GetType() == GetContentType(targetType))
                {
                    return true;
                }
            }
            return false;
        }

        private List<Vector2Int> RotatePattern(List<Vector2Int> positions, Vector2Int direction)
        {
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
            return rotatedPositions;
        }

        private bool EvaluatePattern(Vector2Int parentPos, Vector2Int direction, List<Vector2Int> pattern)
        {
            List<Vector2Int> rotatedList = RotatePattern(pattern, direction);

            foreach (Vector2Int pos in rotatedList)
            {
                if(GridPresenter.Instance.GetContent(pos + parentPos).GetType() == GetContentType(targetType))
                {
                    return true;
                }
            }
            //TODO auf rotatedPositions parentPos drauf addieren
            //TODO checken ob target Bedingung erfüllt und dann returnen
            //Liste speichern und dann highlights über Grid visualisieren und später weg machen wieder
            return false; 
        }

        private HashSet<AGridContent> GetTargetsFromPattern(Vector2Int parentPos, Vector2Int direction, List<Vector2Int> pattern)
        {
            HashSet<AGridContent> targets = new HashSet<AGridContent>();
            List<Vector2Int> rotatedList = RotatePattern(pattern, direction);

            foreach (Vector2Int pos in rotatedList)
            {
                AGridContent content = GridPresenter.Instance.GetContent(parentPos + pos);
                if (content.GetType() == GetContentType(targetType))
                {
                    targets.Add(content);
                }
            }
            return targets;
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

        private void DisableHighlights()
        {
            GridPresenter.Instance.DisableAllGridHighlights();
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
                    targets.AddRange(GetTargetsFromPattern(parentPos, Vector2Int.up, patternO));
                    break;
                case ActionDirection.L:
                    targets.AddRange(GetTargetsFromPattern(parentPos, direction, patternL));
                    break;
                case ActionDirection.T:
                    targets.AddRange(GetTargetsFromPattern(parentPos, direction, patternT));
                    break;
                case ActionDirection.X:
                    targets.AddRange(GetTargetsX(parentPos, length));
                    break;
                case ActionDirection.cross:
                    targets.AddRange(GetTargetsFromPattern(parentPos, direction, patternCross));
                    break;
                case ActionDirection.middleFinger:
                    targets.AddRange(GetTargetsFromPattern(parentPos, direction, patternMiddleFinger));
                    break;
            }
            return targets;
        }

        private IEnumerable<AGridContent> GetTargetsX(Vector2Int parentPos, int length)
        {
            HashSet<AGridContent> targets = new HashSet<AGridContent>();
            for (int i = 0; i > length; i++)
            {
                AGridContent content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.left * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    targets.Add(content);
                }

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.left * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    targets.Add(content);
                }

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.right * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    targets.Add(content);
                }

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.right * i);
                if (content.GetType() == GetContentType(targetType))
                {
                    targets.Add(content);
                }
            }
            return targets;
        }



        public void Cast(Action callbackCastFinished)
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
                        Debug.Log("Attack Successful");
                        break;
                    case CommunityContent community:
                        if(community.communityPresenter.GetFaction() == Unit.UnitModel.Faction.None)
                        {
                            if (community.communityPresenter.IsCaptureSuccessful())
                            {
                                //TODDO: Success Animations
                                Debug.Log("IsCapture Success");
                                community.communityPresenter.SetFaction(GetComponentInParent<Unit.UnitPresenter>().GetFaction());
                            }
                        } else
                        {
                            if (community.communityPresenter.IsCaptureSuccessful())
                            {
                                //TODO: Success Animations
                                Debug.Log("Capture Success");
                                community.communityPresenter.SetFaction(GetComponentInParent<Unit.UnitPresenter>().GetFaction());
                            } else
                            {
                                //TODO: Failure Animation
                                Debug.Log("Capture Success");
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