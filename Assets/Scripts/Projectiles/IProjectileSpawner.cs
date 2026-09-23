public interface IProjectileSpawner 
{
    public BaseProjectile Spawn(ProjectileData data);
    public void Despawn(BaseProjectile projectile); 
}
