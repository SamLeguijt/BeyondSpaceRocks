using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboData_", menuName ="ScriptableObjects/Combo/new ComboData")]
public class ComboData : ScriptableObject
{
    public float Limit;
    public float comboRechargeCooldownSeconds; 
    public float comboIntervalSeconds; 
}
