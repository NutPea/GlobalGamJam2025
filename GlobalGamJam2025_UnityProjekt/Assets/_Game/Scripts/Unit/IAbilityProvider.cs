using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public interface IAbilityProvider
    {
        List<AAbility> GetAbilityPossibilities(Vector2Int position);
    }
}