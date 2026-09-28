public struct InteractionResult
{
    public BaseProjectile Projectile { get; }
    public IProjectileTarget Target { get; }
    public bool ShouldInteract { get; }

    public InteractionResult(BaseProjectile projectile, IProjectileTarget target, bool interact)
    {
        Projectile = projectile;
        Target = target;
        ShouldInteract = interact;
    }
}