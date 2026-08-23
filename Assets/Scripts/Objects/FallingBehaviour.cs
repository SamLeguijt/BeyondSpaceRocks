using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

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

    public void SetFallSpeed(float value)
    {
        FallSpeed = value;
    }

    public virtual void Fall()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - FallSpeed * GameManager.Instance.SimulationSpeed * Time.deltaTime);
    }

    void Update()
    {
        if (!IsActive)
           return; 
        
        Fall();
    }
}
