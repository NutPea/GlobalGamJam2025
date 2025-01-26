using Game.Grid;
using Game.Grid.Content;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Spawner
{
    [ExecuteInEditMode]

    public class SpawnerPresenter : MonoBehaviour, ITarget
    {
        [SerializeField, Range(0.05f, 5)] private float turnFocusDuration;
        [SerializeField, ] private List<Vector2Int> spawnPositionsRelative;

        [SerializeField] private int startAbilityPoints;
        [SerializeField] private int abilityPointsPerRound;
        [SerializeField] private int abilityPointsToSpawnPrefab;
        [SerializeField] private int initiative;

        private int currentAbilityPoints;
        [SerializeField] private GameObject unitCellPrefabReference;

        [SerializeField] private GameObject spawnVisualPrefab;
        [SerializeField] private GameObject spawnVisualParent;

        private void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            currentAbilityPoints = startAbilityPoints;
        }
        public void UpdateSpawner()
        {
            currentAbilityPoints += abilityPointsPerRound;
            if(currentAbilityPoints >= abilityPointsToSpawnPrefab)
            {
                currentAbilityPoints -= abilityPointsToSpawnPrefab;
                foreach(Vector2Int spawnPositionRelative in spawnPositionsRelative)
                {
                    Vector2Int spawnPosition = spawnPositionRelative + Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));
                    if (GridPresenter.Instance.GetContent(spawnPosition) is EmptyContent)
                    {
                        unitCellPrefabReference.SetActive(false);
                        UnitContent unitContent = Instantiate(unitCellPrefabReference, transform.parent).GetComponent<UnitContent>();
                        unitCellPrefabReference.SetActive(true);
                        unitContent.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.y);
                        unitContent.gameObject.SetActive(true);

                        GridPresenter.Instance.ReplaceCell(spawnPosition, unitContent);
                        break;
                    }
                }                
            }
        }
        public float GetTurnFocusDuration()
        {
            return turnFocusDuration;
        }
        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            foreach(Transform t in spawnVisualParent.transform)
            {
                DestroyImmediate(t.gameObject);
            }
            foreach (Vector2Int spawnPositionRelative in spawnPositionsRelative)
            {
                Vector2Int spawnPosition = spawnPositionRelative + Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));
                GameObject newSpawn = Instantiate(spawnVisualPrefab, spawnVisualParent.transform);
                newSpawn.transform.position = new Vector3(spawnPosition.x, 0.05f, spawnPosition.y);
            }
        }

        public int GetInitiative()
        {
            return initiative;
        }
    }
}