using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBehaviour : MonoBehaviour
{
// fall behaviours
// IProjectiletarget 
// DestructibleObjectBase uses above classes
// Rock is a destructible object, using IColorSomething interface 
    
    [field: SerializeField] public float FallSpeed { get; private set; } = 1f;
    
    public bool IsActive { get; private set; } = false;

    public void ToggleActive()
    {
        SetActive(!IsActive);
    }

    public void SetActive(bool value)
    {
        IsActive = value;
    }

    public virtual void Fall()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - FallSpeed * GameManager.Instance.SimulationSpeed * Time.deltaTime);
    }

    protected virtual void FixedUpdate()
    {
        if (IsActive)
            Fall();
    } 
}
