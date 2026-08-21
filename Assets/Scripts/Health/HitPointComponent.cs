using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointComponent : MonoBehaviour
{
    [field: SerializeField]  public int MaxHP { get; private set; }

    [field: SerializeField] public int CurrentHP { get; private set; }

    public void TakeHit(int amount)
    {
        
    }

    public void RestoreHit(int amount)
    {
        
    }

    public void ResetHP()
    {
        
    }

    public void Kill()
    {
        
    }
}
