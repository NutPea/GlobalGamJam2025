using System.Collections.Generic;
using UnityEngine;
using static Game.Unit.UnitModel;

namespace Game.Community
{
    public class CommunityPresenter : MonoBehaviour, ITarget
    {
        [SerializeField] private int initiative;
        private CommunityModel model;

        [Header("VeganComments")]
        [SerializeField] public List<string> positivVeganComments = new List<string>();
        [SerializeField] public List<string> negativVeganComments = new List<string>();

        [Header("BavariaComments")]
        [SerializeField] public List<string> positivBavariaComments = new List<string>();
        [SerializeField] public List<string> negativBavariaComments = new List<string>();

        [Header("AluHeadComments")]
        [SerializeField] public List<string> positivAluHeadComments = new List<string>();
        [SerializeField] public List<string> negativAluHeadComments = new List<string>();

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

        public int GetInitiative()
        {
            return initiative;
        }
    }
}