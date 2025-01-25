using Game.Unit;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Grid
{
    [RequireComponent(typeof(GridModel))]
    public class GridPresenter : MonoBehaviour
    {

        public static GridPresenter Instance;
        private GridModel model;

        private void OnEnable()
        {
            Instance = this;
            model = GetComponent<GridModel>();
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

        public int GetRoundCount()
        {
            return model.GetRoundCount();
        }

    }
}