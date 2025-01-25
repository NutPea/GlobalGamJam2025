using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class OneFieldMovementProvider : MonoBehaviour, IMovementProvider
    {
        public HashSet<Vector2Int> GetMovementPossibilites(Vector2Int position)
        {
            HashSet<Vector2Int> returnMoves = new HashSet<Vector2Int>();

            if (CheckPosition(position + Vector2Int.up))
            {
                returnMoves.Add(position);
            }
            if (CheckPosition(position + Vector2Int.down))
            {
                returnMoves.Add(position);
            }
            if (CheckPosition(position + Vector2Int.left))
            {
                returnMoves.Add(position);
            }
            if (CheckPosition(position + Vector2Int.right))
            {
                returnMoves.Add(position);
            }

            return returnMoves;
        }

        private bool CheckPosition(Vector2Int position)
        {
            switch (Grid.GridPresenter.Instance.GetContent(position))
            {
                case Game.Grid.Content.EmptyContent unit:
                    return true;
            }
            return false;
        }
    }
}