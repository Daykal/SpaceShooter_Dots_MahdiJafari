using UnityEngine;
using Unity.Entities;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject EnemeyPrefab;

    public int EnemiesToSpawnPerSecond = 10;
    public int EnmiesToSpawnIncrementAmount = 1;
    public int MaxEnemeisToSpawnPerSecond = 20;
    public float EnemySpawnRadius = 30f;
    public float MinDistFromPlayer = 8f;
    public float TimeBeforeNextSpawn = 3;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity enemySpawnerEntity = GetEntity(TransformUsageFlags.None);
            AddComponent(enemySpawnerEntity, new EnemySpawnerComponent 
            {
                EnemeyPrefab = GetEntity(authoring.EnemeyPrefab, TransformUsageFlags.None),
                EnemiesToSpawnPerSecond = authoring.EnemiesToSpawnPerSecond,
                EnmiesToSpawnIncrementAmount = authoring.EnmiesToSpawnIncrementAmount,
                MaxEnemeisToSpawnPerSecond = authoring.MaxEnemeisToSpawnPerSecond,
                EnemySpawnRadius = authoring.EnemySpawnRadius,
                MinDistFromPlayer=authoring.MinDistFromPlayer,
                TimeBeforeNextSpawn=authoring.TimeBeforeNextSpawn,

            });
        }
    }
}