using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class EmptyMovementProvider : MonoBehaviour, IMovementProvider
    {
        public HashSet<Vector2Int> GetMovementPossibilites(Vector2Int position)
        {

            return new HashSet<Vector2Int>();
        }
    }
}