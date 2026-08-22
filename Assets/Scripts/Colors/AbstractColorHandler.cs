using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName ="ColorHandler_", menuName = "ScriptableObjects/Color/new ColorHandler")]
public abstract class AbstractColorHandler : ScriptableObject
{
    [field: SerializeField] public ColorData BaseColorData { get; protected set; }

    public abstract bool ResolveColorInteraction(ColorData colorData); 
}

[CreateAssetMenu(fileName ="ColorHandler_Default", menuName = "ScriptableObjects/Color/new SingleEqualsColorInteractionHandler")]
public class SingleEqualsColorInteractionHandler : AbstractColorHandler
{
    public override bool ResolveColorInteraction(ColorData colorData)
    {
        return BaseColorData.ColorType == colorData.ColorType; 
    }
}