using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDestructionResponse : ScriptableObject
{
    public abstract void HandleResponse(DestructibleObject destroyed, Projectile by);
}
