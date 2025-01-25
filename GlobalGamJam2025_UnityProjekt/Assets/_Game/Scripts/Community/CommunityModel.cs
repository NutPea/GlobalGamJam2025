using UnityEngine;
using static Game.Unit.UnitModel;

namespace Game.Community
{
    public class CommunityModel : MonoBehaviour
    {
        [SerializeField] private Faction _faction;
        [SerializeField] private float _rollSuccessProbability;
        [SerializeField] private int _pointsEndOfRound;

        public ModelEntry<Faction> faction = new ();
        public ModelEntry<float> rollSuccessProbability = new();

        public void InitValues()
        {
            faction.Value = _faction;
            rollSuccessProbability.Value = _rollSuccessProbability;
        }
    }
}
