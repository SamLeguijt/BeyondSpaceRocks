using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MovementBehaviour : MonoBehaviour
{
    [field: SerializeField] public Vector2 Direction { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1f;
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
        MoveSpeed = value;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction.normalized;
    }

    protected virtual void Move()
    {
        float delta = MoveSpeed
                    * GameManager.Instance.SimulationSpeed
                    * Time.deltaTime;

        transform.position += (Vector3)(Direction * delta);
    }

    void Update()
    {
        if (!IsActive)
           return; 
        
        Move();
    }
}
