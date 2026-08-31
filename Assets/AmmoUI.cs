using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ColorUtility = Unity.VisualScripting.ColorUtility;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weaponController;
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
            
        UpdateAmmoRoundsUI(currentAmmo, maxAmmo);
        UpdateDisplayText(currentAmmo, maxAmmo);
    }

    private void OnWeaponReloadEvent(AbstractWeapon weapon)
    {
        int maxAmount = weapon.WeaponData.MaxAmmo; 
        int available = weapon.CurrentAmmo;

        UpdateAmmoRoundsUI(available, maxAmount);
        UpdateDisplayText(available, maxAmount);

    }

    private void OnEquipWeaponEvent(AbstractWeapon weapon)
    {
        int maxAmount = weapon.WeaponData.MaxAmmo; 
        int available = weapon.CurrentAmmo;

        SetupAmmoRoundsUI(available, maxAmount);
        UpdateDisplayText(available, maxAmount);

    }

    private void UpdateDisplayText(int availableRoundsAmount, int maxRoundsAmount)
    {
        string result = ""; 

        string availableAmmoText = availableRoundsAmount.ToString();
        string maxAmmoText = maxRoundsAmount.ToString();

        if (availableRoundsAmount < 10)
            availableAmmoText = $"0{availableAmmoText}";

        if (maxRoundsAmount < 10)
            maxAmmoText = $"0{maxAmmoText}";

        
        Color32 available = availableRoundColor;
        Color32 unavailable = unavailableRoundColor;

        string availableHex = $"{available.r:X2}{available.g:X2}{available.b:X2}{available.a:X2}";
        string unavailableHex = $"{unavailable.r:X2}{unavailable.g:X2}{unavailable.b:X2}{unavailable.a:X2}";

        if (availableRoundsAmount == 0)
        {
            result =    $"<color=#{unavailableHex}>{availableAmmoText}</color>" +
                        $"<color=#{availableHex}> | {maxAmmoText}</color>";
        }
        else
        {
            result =    $"<color=#{availableHex}>{availableAmmoText}</color>" +
                        $"<color=#{availableHex}> | {maxAmmoText}</color>";
        }

        textAsset.text = result;
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

            Image ammoRound = Instantiate(ammoRoundImage, position, Quaternion.identity, roundsUiOrigin);
            Color availability = GetImageTargetColor(i, available);
            SetImageColor(ammoRound, availability);

            roundsUI.Add(ammoRound);
        }
    }
}
