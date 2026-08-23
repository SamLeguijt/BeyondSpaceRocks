using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : DestructibleObject
{
    protected override void HandleDestruction()
    {
        
    }


    /// NEXT STEPS:
    /// - DestructionResponse abstract class 
    /// - Takes DestructableObject and (for now) Projectile 
    /// - AddScoreResponse implementation 
    /// - Evaluate Projectile and further damage/hit abstractionsß
}
