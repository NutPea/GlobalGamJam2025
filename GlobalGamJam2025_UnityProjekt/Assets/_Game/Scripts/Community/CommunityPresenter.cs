using UnityEngine;
using static Game.Unit.UnitModel;

namespace Game.Community
{
    public class CommunityPresenter : MonoBehaviour, ITarget
    {
        private CommunityModel model;

        private void Awake()
        {
            model = GetComponent<CommunityModel>();
        }
        private void OnEnable()
        {
            model.InitValues();
        }

        public Faction GetFaction()
        {
            return model.faction.Value;
        }
        public void SetFaction(Faction newFaction)
        {
            model.faction.Value = newFaction;
        }
       
        public bool IsCaptureSuccessful()
        {
            if(model.faction.Value == Faction.None) return true;

            return Random.Range(0f, 1f) < model.rollSuccessProbability.Value;
        }

        public float GetTurnFocusDuration()
        {
            return 1;
        }

        public void UpdateCommunity()
        {
            return;
        }
    }
}