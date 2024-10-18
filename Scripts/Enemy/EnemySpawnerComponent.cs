using Unity.Entities;

public struct EnemySpawnerComponent : IComponentData
{
    public Entity EnemeyPrefab;

    public int EnemiesToSpawnPerSecond;
    public int EnmiesToSpawnIncrementAmount;
    public int MaxEnemeisToSpawnPerSecond;
    public float EnemySpawnRadius;
    public float MinDistFromPlayer;
    public float TimeBeforeNextSpawn;
    public float CurrentTimeBeforeNextSpawn;
}
