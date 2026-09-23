using UnityEngine;

public enum EInteractionRule
{
    MatchColor,
    IgnoreColor
}

public interface IProjectile 
{
    Vector3 Position { get;  }
    EInteractionRule InteractionRule { get; }
    EProjectileCollisionResponse CollisionResponse { get; }
    ColorData ColorData { get; }
}
