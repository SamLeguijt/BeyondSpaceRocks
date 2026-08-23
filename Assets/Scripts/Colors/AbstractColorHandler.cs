using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName ="ColorHandler_", menuName = "ScriptableObjects/Color/new ColorHandler")]
public abstract class AbstractColorHandler : ScriptableObject
{
    [field: SerializeField] public ColorData BaseColorData { get; protected set; }

    public abstract bool ResolveColorInteraction(ColorData a, ColorData b); 
}

[CreateAssetMenu(fileName ="ColorHandler_Default", menuName = "ScriptableObjects/Color/new SingleEqualsColorInteractionHandler")]
public class SingleEqualsColorInteractionHandler : AbstractColorHandler
{
    public override bool ResolveColorInteraction(ColorData a, ColorData b)
    {
        return a.ColorType == b.ColorType; 
    }
}


/// --- NEXT STEPS ---
/// Make SO Instance of ^^
/// Make Rock class dericing from DestructibleObject
/// Rock class handles FallingBehaviour and lives. 