using UnityEngine;

namespace Game.Grid
{
    public class GridModel : MonoBehaviour
    {
        [SerializeField] private AGridContent fallbackContent;
        [SerializeField] private int roundCount;

        private AGridContent[,] content;

        private void OnEnable()
        {
            InitGrid();
        }
        private void InitGrid()
        {
            content = new AGridContent[0, 0];
        }

        public void SwapCells(Vector2Int posA, Vector2Int posB)
        {
            AGridContent contentA = GetContent(posA);
            AGridContent contentB = GetContent(posB);

            content[posA.x, posA.y] = contentB;
            content[posB.x, posB.y] = contentA;
        }
        public AGridContent GetContent(Vector2Int position)
        {
            if(position.x <0 || position.x > content.GetLength(0))
            {
                return fallbackContent;
            }
            if (position.y < 0 || position.x > content.GetLength(1))
            {
                return fallbackContent;
            }
            return content[position.x, position.y];
        }
        public int GetRoundCount()
        {
            return roundCount;
        }
    }
}