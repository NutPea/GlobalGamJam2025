using Game.Grid.Content;
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
        [SerializeField] private Transform emptyPrefab;
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
        public List<T> GetAll<T>()
        {
            return GetComponentsInChildren<T>().ToList();
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
        public void DestroyCell(Vector2Int position)
        {
            AGridContent newEmpty = Instantiate(emptyPrefab, transform).GetComponentInChildren<AGridContent>();
            newEmpty.transform.position = new Vector3(position.x, 0, position.y);
            ReplaceCell(position, newEmpty);
        }
        public void ReplaceCell(Vector2Int position, AGridContent content)
        {
            GameObject.Destroy(model.GetContent(position).gameObject);
            model.Replace(position, content);
        }
        public int GetRoundCount()
        {
            return model.GetRoundCount();
        }

        public bool IsWalkable(Vector2Int position)
        {
            return model.GetContent(position) is EmptyContent;
        }

    }
}