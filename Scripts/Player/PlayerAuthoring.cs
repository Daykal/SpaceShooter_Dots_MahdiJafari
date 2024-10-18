using UnityEngine;
using Unity.Entities;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed = 4;
    public GameObject BulletPrefab;
    public int BulletToSpawn = 10;
    [Range(0, 5)] public float BulletSpread = 2f;


    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.None);

            AddComponent(playerEntity, new PlayerComponent
            {
                Speed = authoring.MoveSpeed,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.None),
                BulletToSpawn = authoring.BulletToSpawn,
                BulletSpread = authoring.BulletSpread
                
            });
        }
    }
}
