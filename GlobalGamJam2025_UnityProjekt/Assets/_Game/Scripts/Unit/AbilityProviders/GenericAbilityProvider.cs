using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class GenericAbilityProvider : MonoBehaviour, IAbilityProvider
    {
        public List<AAbility> GetAbilityPossibilities(Vector2Int position)
        {
            List<AAbility> abilityList = new List<AAbility>();
            abilityList.AddRange(GetComponentsInChildren<AAbility>());

            return abilityList;
        }
    }
}