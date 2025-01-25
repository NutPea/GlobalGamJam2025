using Game.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Grid
{
    [RequireComponent(typeof(GridModel))]
    public class GridPresenter : MonoBehaviour
    {
        public event Action<Vector2Int> OnGridPressed;

        public static GridPresenter Instance;
        private GridModel model;

        private void OnEnable()
        {
            Instance = this;
            model = GetComponent<GridModel>();
        }

        public void InvokeGridPressed(Vector2Int position)
        {
            OnGridPressed?.Invoke(position);
        }

        public AGridContent GetContent(Vector2Int position)
        {
            return model.GetContent(position);
        }
        public List<UnitPresenter> FindGetUnits()
        {
            return GetComponentsInChildren<UnitPresenter>().ToList();
        }

        public void DisableAllGridHighlights()
        {
            foreach (AGridContent content in GetComponentsInChildren<AGridContent>().ToList())
            {
                content.SetHighlightOption(AGridContent.HighlightOption.None);
            }
        }

        public void SwapCells(Vector2Int posA, Vector2Int posB)
        {
            model.SwapCells(posA, posB);
        }
        public int GetRoundCount()
        {
            return model.GetRoundCount();
        }

    }
}