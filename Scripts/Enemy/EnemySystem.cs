using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;


public partial struct EnemySystem : ISystem
{
    private EntityManager _entityManager;

    private Entity _playerEntity;


    public void OnUpdate(ref SystemState state) 
    {
        _entityManager = state.EntityManager;

        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

        NativeArray<Entity> allEntities = _entityManager.GetAllEntities();

        foreach (Entity entity in allEntities) 
        {
            if (_entityManager.HasComponent<EnemyComponent>(entity)) 
            {
                LocalTransform enemyTransform = _entityManager.GetComponentData<LocalTransform>(entity);
                EnemyComponent enemyComponent = _entityManager.GetComponentData<EnemyComponent>(entity);
                float3 moveDirection = math.normalize(playerTransform.Position - enemyTransform.Position);

                enemyTransform.Position += enemyComponent.Speed * SystemAPI.Time.DeltaTime * moveDirection;

                //looking at player
                float3 direction = math.normalize(playerTransform.Position - enemyTransform.Position);
                float angle = math.atan2(direction.y, direction.x);
                angle -= math.radians(90f);
                quaternion lookRotation = quaternion.AxisAngle(new float3(0, 0, 1), angle);
                enemyTransform.Rotation = lookRotation;

                _entityManager.SetComponentData(entity, enemyTransform);
            }
        } 

    }

}
