using UnityEngine;

namespace Game.Grid
{
    [RequireComponent(typeof(GridModel))]
    public class GridPresenter : MonoBehaviour
    {

        public static GridPresenter Instance;
        private GridModel model;

        private void Awake()
        {
            Instance = this;
            model = GetComponent<GridModel>();
        }


        public AGridContent GetContent(Vector2Int position)
        {
            return model.GetContent(position);
        }


    }
}