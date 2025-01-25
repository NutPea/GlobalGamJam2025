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
                returnMoves.Add(position + Vector2Int.up);
            }
            if (CheckPosition(position + Vector2Int.down))
            {
                returnMoves.Add(position + Vector2Int.down);
            }
            if (CheckPosition(position + Vector2Int.left))
            {
                returnMoves.Add(position + Vector2Int.left);
            }
            if (CheckPosition(position + Vector2Int.right))
            {
                returnMoves.Add(position + Vector2Int.right);
            }

            return returnMoves;
        }

        private bool CheckPosition(Vector2Int position)
        {
            switch (Grid.GridPresenter.Instance.GetContent(position))
            {
                case Grid.Content.EmptyContent:
                    return true;
            }
            return false;
        }
    }
}