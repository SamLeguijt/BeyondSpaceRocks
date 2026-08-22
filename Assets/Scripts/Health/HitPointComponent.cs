using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointComponent : MonoBehaviour
{
    [field: SerializeField]  public int MaxHP { get; private set; }

    [field: SerializeField] public int CurrentHP { get; private set; }

    public bool IsAlive => (CurrentHP > 0); 
    
    public delegate void HitPointChangesHandler(int amountChanged); 
    public event HitPointChangesHandler OnHitTakenEvent;
    public event HitPointChangesHandler OnHitRestoredEvent; 
    public event HitPointChangesHandler OnDeathEvent; 

    public virtual void ResetHP()
    {
        RestoreHitPoints(MaxHP);
    }

    public virtual void RestoreHitPoints(int amount)
    {
        int amountRestored = Math.Min(MaxHP - CurrentHP, amount);
    
        CurrentHP = Math.Min(MaxHP, CurrentHP + amountRestored);
        OnHitRestoredEvent?.Invoke(amountRestored);
    }

    public virtual void TakeHitPoints(int hits)
    {
        if (!CanTakeHit())
            return;

        int amountTaken = Math.Max(CurrentHP, hits); 

        CurrentHP = Math.Max(0, CurrentHP - amountTaken); 
        OnHitTakenEvent?.Invoke(amountTaken);

        if (CurrentHP == 0)
            OnDeath(amountTaken); 
    }

    public virtual bool CanTakeHit()
    {
        return IsAlive; 
    }

    public virtual void Kill()
    {
        TakeHitPoints(MaxHP);
    }

    protected virtual void OnDeath(int lastHit)
    {
        OnDeathEvent?.Invoke(lastHit);
    }
}
