using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private GameObject ammoPanelUI; 
    [SerializeField] private TextMeshProUGUI textAsset; 
    [SerializeField] private Slider sliderAsset; 
    [SerializeField] private Image ammoRoundImage; 

    [SerializeField] private Color disabledRoundColor; 

    private bool isSetUp = false;
    private int currentAmmo = 0;
    private int maxAmmo = 0; 

    private Stack<Image> ammoRoundsStack = new();
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
        currentAmmo = weaponController.CurrentWeapon.CurrentAmmo;
        maxAmmo = weaponController.CurrentWeapon.WeaponData.MaxAmmo;


        if (!isSetUp)
            SetupAmmoRoundsUI(maxAmmo);
            
        UpdatePanelUI();
    }

    private void UpdatePanelUI()
    {
        textAsset.text = $"[{currentAmmo} / {maxAmmo}]";

        RemoveAmmoRound();
    }

    private void RemoveAmmoRound()
    {
        Image round = ammoRoundsStack.Pop();

        if (round != null)
        {
            round.color = disabledRoundColor;
        }
    }

    private void SetupAmmoRoundsUI(int maxAmmoRounds)
    {
        Vector2 originPos = sliderAsset.transform.position; 
        float yOffset = 10f; 


        for (int i = 0; i < maxAmmoRounds; i++)
        {
            float yPos = originPos.y + (yOffset * i); 
            Vector2 position = new Vector2(originPos.x, yPos);

            Image ammoRound = Instantiate(ammoRoundImage, position, Quaternion.identity, ammoPanelUI.transform);
            ammoRoundsStack.Push(ammoRound);
        }

        if (!isSetUp)
            isSetUp = true;
    }
}
