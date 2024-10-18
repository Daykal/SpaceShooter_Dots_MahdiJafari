using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public float Speed;
    public Entity BulletPrefab;
    public int BulletToSpawn;
    public float BulletSpread;

}
