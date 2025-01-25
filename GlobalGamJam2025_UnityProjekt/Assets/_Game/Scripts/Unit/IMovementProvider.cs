using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public interface IMovementProvider
    {
        /// <summary>
        /// Gibt Hashset an Tats�chlich begehbaren Positionen zur�ck -> an Grid anfragen ob frei ist bevor ich was zur�ck gebe
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        HashSet<Vector2Int> GetMovementPossibilites(Vector2Int position);
    }
}