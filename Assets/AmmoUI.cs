using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weaponController;

    private int ammoToDisplay = 0;


    void OnEnable()
    {
        if (weaponController != null)
        {
            weaponController.weaponFireEvent += OnPlayerFireEvent;
            
        }
    }
    
    void OnDisable()
    {
        if (weaponController != null)
        {
            weaponController.weaponFireEvent -= OnPlayerFireEvent;
        }
    }



    private void OnPlayerFireEvent(AbstractWeapon weapon)
    {
        ammoToDisplay = weaponController.CurrentWeapon.CurrentAmmo;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        
    }
}
