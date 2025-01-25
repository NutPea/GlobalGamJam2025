using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Grid
{
    public class GridModel : MonoBehaviour
    {
        [SerializeField] private AGridContent fallbackContent;
        [SerializeField] private int roundCount;

        private Dictionary<Vector2Int, AGridContent> content = new();

        private void OnEnable()
        {
            InitGrid();
        }
        private void InitGrid()
        {
            List<AGridContent> contentList = GetComponentsInChildren< AGridContent>().ToList();
            content = new Dictionary<Vector2Int, AGridContent>();
            contentList.ForEach(c => {
                Vector2Int result = Vector2Int.RoundToInt(new Vector2(c.transform.position.x, c.transform.position.z));
                content.Add(result, c);
            });
        }

        public void SwapCells(Vector2Int posA, Vector2Int posB)
        {
            AGridContent contentA = GetContent(posA);
            AGridContent contentB = GetContent(posB);

            content[posA] = contentB;
            content[posB] = contentA;
        }
        public AGridContent GetContent(Vector2Int position)
        {
            if(!content.ContainsKey(position)) return fallbackContent;
            return content[position];
        }
        public int GetRoundCount()
        {
            return roundCount;
        }
    }
}