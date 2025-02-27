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
                if(content.ContainsKey(result))
                {
                    GameObject.Destroy(content[result]);                    
                }
                content[result] = c;
            });
        }

        public void SwapCells(Vector2Int posA, Vector2Int posB)
        {
            AGridContent contentA = GetContent(posA);
            AGridContent contentB = GetContent(posB);

            content[posA] = contentB;
            content[posB] = contentA;

            contentA.transform.position = new Vector3(posB.x, 0, posB.y);
            contentB.transform.position = new Vector3(posA.x, 0, posA.y);
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
        public void Replace(Vector2Int position, AGridContent gridContent)
        {
            content[position] = gridContent;
        }
    }
}