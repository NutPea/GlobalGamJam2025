using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Grid
{
    public class GridModel : MonoBehaviour
    {
        [SerializeField] private AGridContent fallbackContent;

        private AGridContent[,] content;

        private void Awake()
        {
            InitGrid();
        }
        private void InitGrid()
        {
            content = new AGridContent[0, 0];
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
    }
}