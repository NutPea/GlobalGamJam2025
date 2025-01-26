using Game.Grid;
using Game.Grid.Content;
using Game.Unit;
using GetraenkeBub;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

        [Header("Attack Reaktions")]
        public List<Sprite> showAttackSprites;
        public List<string> showAttackText;
        public List<Sprite> attackReaktions;

        private UnitModel.Faction myFaction;

        /// <summary>
        /// ignoriert Kosten -> überprüft ob im Grid Target im AOE Bereich verfügbar ist
        /// </summary>
        /// <returns></returns>
        /// 
        private List<Vector2Int> patternT = new List<Vector2Int> { Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up * 4, Vector2Int.up * 4 + Vector2Int.left, Vector2Int.up * 4 + Vector2Int.right };
        private List<Vector2Int> patternL = new List<Vector2Int> { Vector2Int.up , Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up *3 + Vector2Int.left };
        private List<Vector2Int> patternCross = new List<Vector2Int> { Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up * 4, Vector2Int.up * 3 + Vector2Int.left, Vector2Int.up * 3 + Vector2Int.right };
        private List<Vector2Int> patternMiddleFinger = new List<Vector2Int> { Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right, Vector2Int.right, Vector2Int.left };
        private List<Vector2Int> patternO = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right, Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right };
        private List<Vector2Int> patternCircle = new List<Vector2Int> { Vector2Int.up + Vector2Int.right, Vector2Int.up + Vector2Int.left, Vector2Int.down + Vector2Int.right, Vector2Int.down + Vector2Int.left,Vector2Int.up, Vector2Int.up * 2, Vector2Int.down, Vector2Int.down * 2, Vector2Int.left, Vector2Int.left * 2, Vector2Int.right, Vector2Int.right * 2 };

        private void Start()
        {
            UIStateManager.Instance.OnAbilityHighlight += GetOnAbilityHighlight;
            UIStateManager.Instance.OnAbilityStop += OnAbilityStop;
            myFaction = GetComponentInParent<UnitPresenter>().GetFaction();
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
                    HighLightPattern(parentPos, Vector2Int.up, patternCircle);
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
            for (int i = 1; i <= length; i++)
            {
                AGridContent content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.left * i);
                content.SetHighlightOption(AGridContent.HighlightOption.Ability);

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.left * i);
                content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.right * i);
                content.SetHighlightOption(AGridContent.HighlightOption.Ability);
                

                content = GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.right * i);
                content.SetHighlightOption(AGridContent.HighlightOption.Ability);
            }
        }

        private void HighLightStraightDirection(Vector2Int parentPos, int length, Vector2Int direction)
        {
            for (int i = 1; i <= length; i++)
            {
                AGridContent content = GridPresenter.Instance.GetContent(parentPos + direction * i);
                content.SetHighlightOption(AGridContent.HighlightOption.Ability);
            }
        }

        private void HighLightPattern(Vector2Int parentPos, Vector2Int direction, List<Vector2Int> pattern)
        {
            List<Vector2Int> rotatedList = RotatePattern(pattern, direction);

            foreach (Vector2Int pos in rotatedList)
            {
                AGridContent content = GridPresenter.Instance.GetContent(pos + parentPos);
                content.SetHighlightOption(AGridContent.HighlightOption.Ability);
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
                    return EvaluatePattern(parentPos, Vector2Int.up, patternCircle);
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
                case ActionDirection.skip:
                    return true;
            }

            return false;
        }

        private bool EvaluateX(Vector2Int parentPos, int length)
        {
            for ( int i = 1; i <= length; i++)
            {
                bool compareFactions = CompareToMyFaction(parentPos + Vector2Int.up * i + Vector2Int.left * i); 
                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.left * i).GetType() == GetContentType(targetType) && compareFactions)
                {
                    return true;
                }

                compareFactions = CompareToMyFaction(parentPos + Vector2Int.down * i + Vector2Int.left * i);
                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.down * i + Vector2Int.left * i).GetType() == GetContentType(targetType) && compareFactions)
                {
                    return true;
                }

                compareFactions = CompareToMyFaction(parentPos + Vector2Int.up * i + Vector2Int.right * i);
                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.right * i).GetType() == GetContentType(targetType) && compareFactions)
                {
                    return true;
                }

                compareFactions = CompareToMyFaction(parentPos + Vector2Int.up * i + Vector2Int.left * i);
                if (GridPresenter.Instance.GetContent(parentPos + Vector2Int.up * i + Vector2Int.left * i).GetType() == GetContentType(targetType) && compareFactions)
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
                bool compareFactions = CompareToMyFaction(pos + parentPos);
                if (GridPresenter.Instance.GetContent(pos + parentPos).GetType() == GetContentType(targetType) && compareFactions)
                {
                    return true;
                }
            }
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
                bool compareFactions = CompareToMyFaction(parentPos + direction * i);

                if (GridPresenter.Instance.GetContent(parentPos + direction * i).GetType() == GetContentType(targetType) && compareFactions)
                {
                    return true;
                }

            }
            return false;
        }

        private bool CompareToMyFaction(Vector2Int pos)
        {
            bool compareFactions = false;
            switch (GridPresenter.Instance.GetContent(pos))
            {
                case Grid.Content.UnitContent unit:
                    if (unit != null && myFaction != unit.unitReference.GetFaction())
                    {
                        compareFactions = true;
                    }
                    break;
                case Grid.Content.CommunityContent community:
                    compareFactions = true;
                    break;
            }

            return compareFactions;
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
                    targets.AddRange(GetTargetsFromPattern(parentPos, Vector2Int.up, patternCircle));
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
                case ActionDirection.skip:
                    targets.Add(GridPresenter.Instance.GetContent(parentPos));
                    break;
            }
            return targets;
        }

        private IEnumerable<AGridContent> GetTargetsX(Vector2Int parentPos, int length)
        {
            HashSet<AGridContent> targets = new HashSet<AGridContent>();
            for (int i = 1; i <= length; i++)
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


        List<GameObject> GetGameObjectsFromGridContent(HashSet<AGridContent> aGridContents)
        {
            List<GameObject> returnGameObjects = new List<GameObject> ();

            foreach ( AGridContent content in aGridContents)
            {
                switch (content)
                {
                    case Grid.Content.UnitContent unit:
                        if (unit.unitReference.GetFaction() != myFaction)
                        {
                            returnGameObjects.Add(content.transform.gameObject);
                        }
                        break;
                    case Grid.Content.CommunityContent community:
                        returnGameObjects.Add(content.transform.gameObject);
                        break;
                }     
            }

            return returnGameObjects;
        }

        public void Cast(Action callbackCastFinished)
        {
            if (spawnedEffect != null)
            {
                Instantiate(spawnedEffect, transform.parent.transform);
            }

            if(this.actionDirection == ActionDirection.skip)
            {
                if (UIStateManager.Instance == null)
                {
                    callbackCastFinished();
                }
                else
                {
                    UIStateManager.Instance.HandleAbility(() => callbackCastFinished(), this, transform.parent.gameObject, new List<GameObject> (), false);
                }
            }

            HashSet<AGridContent> targets = GetTargets();
            bool communitySucces = false;
            foreach (var target in targets)
            {
                switch (target)
                {
                    case UnitContent unit:
                        unit.unitReference.ApplyHPChange(-attackDamage);
                        unit.unitReference.SetActionPointChangeModifier(-attackActionPoints);
                        unit.unitReference.SetMaxMovementPointModifier(-attackReduceMovementPoints);
                        break;
                    case CommunityContent community:
                        if(community.communityPresenter.GetFaction() == Unit.UnitModel.Faction.None)
                        {
                            if (community.communityPresenter.IsCaptureSuccessful())
                            {
                                communitySucces = true;
                                community.communityPresenter.SetFaction(GetComponentInParent<Unit.UnitPresenter>().GetFaction());
                            }
                        } else
                        {
                            if (community.communityPresenter.IsCaptureSuccessful())
                            {
                                communitySucces = true;
                                community.communityPresenter.SetFaction(GetComponentInParent<Unit.UnitPresenter>().GetFaction());
                            } else
                            {
                                communitySucces = false;
                            }
                        }
                        break;

                }
            }
            if(UIStateManager.Instance == null)
            {
                callbackCastFinished();
            } else {
                UIStateManager.Instance.HandleAbility(()=>callbackCastFinished(), this, transform.parent.gameObject, GetGameObjectsFromGridContent(targets), communitySucces);
            }
        }
    }
}