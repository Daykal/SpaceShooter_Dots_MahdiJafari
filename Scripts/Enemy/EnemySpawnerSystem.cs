using System.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;


public partial struct EnemySpawnerSystem : ISystem
{
   private EntityManager _entityManager;
   private Entity _enemySpawnerEntity;
   private EnemySpawnerComponent _enemySpawnerComponent;
   private Entity _playerEntity;

   private Unity.Mathematics.Random _random;

    public void OnCreate(ref SystemState state) 
    {
        _random = Unity.Mathematics.Random.CreateFromIndex((uint)_enemySpawnerComponent.GetHashCode());
    }
    
    public void OnUpdate(ref SystemState state) 
    {
        _entityManager = state.EntityManager;

        _enemySpawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerComponent>();
        _enemySpawnerComponent = _entityManager.GetComponentData<EnemySpawnerComponent>(_enemySpawnerEntity);

        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

        SpawnEnemies(ref state);  
    }

    private void SpawnEnemies(ref SystemState state) 
    {
        _enemySpawnerComponent.CurrentTimeBeforeNextSpawn -= SystemAPI.Time.DeltaTime;
        if (_enemySpawnerComponent.CurrentTimeBeforeNextSpawn <= 0f)
        {
            for (int i = 0; i < _enemySpawnerComponent.EnemiesToSpawnPerSecond ; i++)
            {
                EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
                Entity enemyEntity = _entityManager.Instantiate(_enemySpawnerComponent.EnemeyPrefab);

                LocalTransform enemyTransform = _entityManager.GetComponentData<LocalTransform>(enemyEntity);
                LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

                float minDistanceSquared = _enemySpawnerComponent.MinDistFromPlayer * _enemySpawnerComponent.MinDistFromPlayer;
                float2 randomOffset = _random.NextFloat2Direction() * _random.NextFloat(_enemySpawnerComponent.MinDistFromPlayer, _enemySpawnerComponent.EnemySpawnRadius);
                float2 playerPosition = new float2(playerTransform.Position.x, playerTransform.Position.y);
                float2 spawnPosition = playerPosition + randomOffset;
                float distanceSqared = math.lengthsq(spawnPosition - playerPosition);

                if (distanceSqared < minDistanceSquared) 
                {
                    spawnPosition = playerPosition + math.normalize(randomOffset) * math.sqrt(minDistanceSquared);
                }
                enemyTransform.Position = new float3(spawnPosition.x, spawnPosition.y, 0f);

                //looking at player direction
                float3 direction = math.normalize(playerTransform.Position - enemyTransform.Position);
                float angle = math.atan2(direction.y, direction.x);
                angle -= math.radians(90f);
                quaternion lookRotation = quaternion.AxisAngle(new float3(0,0,1), angle);
                enemyTransform.Rotation = lookRotation;

                ECB.SetComponent(enemyEntity, enemyTransform);

                ECB.AddComponent(enemyEntity, new EnemyComponent
                {
                    CurrentHealth = 100f,
                    Speed = 1.2f
                });

                ECB.Playback(_entityManager);
                ECB.Dispose();

            }
            _enemySpawnerComponent.CurrentTimeBeforeNextSpawn = _enemySpawnerComponent.TimeBeforeNextSpawn;
        }

        _entityManager.SetComponentData(_enemySpawnerEntity, _enemySpawnerComponent);
    }

}
