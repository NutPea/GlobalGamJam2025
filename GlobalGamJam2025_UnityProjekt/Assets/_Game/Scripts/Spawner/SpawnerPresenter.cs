using Game.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Spawner
{
    public class SpawnerPresenter : MonoBehaviour, ITarget
    {
        [SerializeField, Range(0.05f, 5)] private float turnFocusDuration;
        [SerializeField, ] private List<Vector2Int> spawnPositions;

        [SerializeField] private int startAbilityPoints;
        [SerializeField] private int abilityPointsPerRound;
        [SerializeField] private int abilityPointsToSpawnPrefab;

        private int currentAbilityPoints;

        private void Awake()
        {
            currentAbilityPoints = startAbilityPoints;
        }
        public void UpdateSpawner()
        {
            currentAbilityPoints += abilityPointsPerRound;
            if(currentAbilityPoints >= abilityPointsToSpawnPrefab)
            {
                currentAbilityPoints -= abilityPointsToSpawnPrefab;
                foreach(Vector2Int spawnPosition in spawnPositions)
                {
                    //if(GridPresenter.Instance.GetContent())
                }
                //spawn a unit
                
            }
        }
        public float GetTurnFocusDuration()
        {
            return turnFocusDuration;
        }
    }
}