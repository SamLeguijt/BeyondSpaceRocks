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
    [SerializeField] private Transform roundsUiOrigin; 
    [SerializeField] private float roundUiOffsetY = 0.1f;
    [SerializeField] private TextMeshProUGUI textAsset; 
    [SerializeField] private Image ammoRoundImage; 

    [SerializeField] private Color availableRoundColor; 
    [SerializeField] private Color unavailableRoundColor; 
    private List<Image> roundsUI = new (); 

    void OnEnable()
    {
        if (weaponController != null)
        {
            weaponController.weaponFireEvent += OnWeaponFireEvent;
            weaponController.weaponReloadEvent += OnWeaponReloadEvent;
            weaponController.equipWeaponEvent += OnEquipWeaponEvent;
        }
    }
    
    void OnDisable()
    {
        if (weaponController != null)
        {
            weaponController.weaponFireEvent -= OnWeaponFireEvent;
            weaponController.weaponReloadEvent -= OnWeaponReloadEvent;
            weaponController.equipWeaponEvent -= OnEquipWeaponEvent;        
        }
    }

    private void OnWeaponFireEvent(AbstractWeapon weapon)
    {
        int currentAmmo = weaponController.CurrentWeapon.CurrentAmmo;
        int maxAmmo = weaponController.CurrentWeapon.WeaponData.MaxAmmo;
            
        UpdateDisplayText(currentAmmo, maxAmmo);
        UpdateAmmoRoundsUI(currentAmmo, maxAmmo);
    }

    private void OnWeaponReloadEvent(AbstractWeapon weapon)
    {
        int maxAmount = weapon.WeaponData.MaxAmmo; 
        int available = weapon.CurrentAmmo;

        UpdateAmmoRoundsUI(available, maxAmount);
    }

    private void OnEquipWeaponEvent(AbstractWeapon weapon)
    {
        int maxAmount = weapon.WeaponData.MaxAmmo; 
        int available = weapon.CurrentAmmo;

        SetupAmmoRoundsUI(available, maxAmount);
    }

    private void UpdateDisplayText(int availableRoundsAmount, int maxRoundsAmount)
    {
        textAsset.text = $"[{availableRoundsAmount} / {maxRoundsAmount}]";
    }

    private void UpdateAmmoRoundsUI(int availableAmount, int maxAmount)
    {
        if (roundsUI.Count != maxAmount)
        {
            SetupAmmoRoundsUI(availableAmount, maxAmount);
            return;
        }

        for (int i = 0; i < maxAmount; i++)
        {
            Image uiImage = roundsUI[i];
            Color targetColor = GetImageTargetColor(i, availableAmount);
            SetImageColor(uiImage, targetColor);
        }
    }

    private void ClearAmmoRoundsUI()
    {
        roundsUI.Clear();
    }   

    private Color GetImageTargetColor(int i, int available)
    {
        Color targetColor = (i < available) ? availableRoundColor : unavailableRoundColor;
        return targetColor;
    }

    private void SetImageColor(Image image, Color target)
    {
        if (image.color != target)
            image.color = target;
    }

    private void SetupAmmoRoundsUI(int available, int max)
    {
        ClearAmmoRoundsUI();

        Vector2 originPos = roundsUiOrigin.position; 
        float yOffset = roundUiOffsetY; 

        for (int i = 0; i < max; i++)
        {
            float yPos = originPos.y + (yOffset * i); 
            Vector2 position = new Vector2(originPos.x, yPos);

            Image ammoRound = Instantiate(ammoRoundImage, position, Quaternion.identity, ammoPanelUI.transform);
            Color availability = GetImageTargetColor(i, available);
            SetImageColor(ammoRound, availability);

            roundsUI.Add(ammoRound);
        }
    }
}
