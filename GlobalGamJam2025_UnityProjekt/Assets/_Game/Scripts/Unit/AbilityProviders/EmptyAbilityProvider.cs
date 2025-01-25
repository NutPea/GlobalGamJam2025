using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class EmptyAbilityProvider : MonoBehaviour, IAbilityProvider
    {
        public List<AAbility> GetAbilityPossibilities(Vector2Int position)
        {
            return new List<AAbility>();
        }
    }
}