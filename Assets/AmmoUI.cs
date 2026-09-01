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
    [SerializeField] private Color currentRoundColor; 
    [SerializeField] private Color refillRoundColor;
    [SerializeField] private Color refillCurrentRoundColor;

    [SerializeField] private float refillFadeDelay = 0.25f;
    [SerializeField] private float refillFadeDuration = 0.05f;

    private Coroutine reloadCoroutine = null;
    private List<Image> roundsUI = new (); 

    void OnEnable()
    {
        if (weaponController != null)
        {
            weaponController.weaponFireEvent += OnWeaponFireEvent;
            weaponController.weaponReloadStartEvent += OnWeaponReloadStartEvent;
            weaponController.weaponReloadFinishedEvent += OnWeaponReloadFinishEvent;
            weaponController.equipWeaponEvent += OnEquipWeaponEvent;
        }
    }
    
    void OnDisable()
    {
        if (weaponController != null)
        {
            weaponController.weaponFireEvent -= OnWeaponFireEvent;
            weaponController.weaponReloadStartEvent -= OnWeaponReloadStartEvent;
            weaponController.weaponReloadFinishedEvent -= OnWeaponReloadFinishEvent;
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

    private void OnWeaponReloadStartEvent(AbstractWeapon weapon, float duration)
    {
        if (reloadCoroutine != null)
            StopCoroutine(reloadCoroutine);

        int refillAmount = weapon.WeaponData.MaxAmmo - weapon.CurrentAmmo;
        reloadCoroutine = StartCoroutine(RefillAmmoRoundsRoutine(duration, refillAmount));
    }

    private void OnWeaponReloadFinishEvent(AbstractWeapon weapon)
    {
        int maxAmount = weapon.WeaponData.MaxAmmo; 
        int available = weapon.CurrentAmmo;

        // UpdateAmmoRoundsUI(available, maxAmount);
        UpdateDisplayText(available, maxAmount);
    }

    private void OnEquipWeaponEvent(AbstractWeapon weapon)
    {
        int maxAmount = weapon.WeaponData.MaxAmmo; 
        int available = weapon.CurrentAmmo;

        SetupAmmoRoundsUI(available, maxAmount);
        UpdateDisplayText(available, maxAmount);
    }

    private IEnumerator RefillAmmoRoundsRoutine(float maxDuration, int roundsToFill)
    {
        WaitForSeconds roundInterval = new WaitForSeconds(maxDuration / roundsToFill);

        int startIndex = roundsUI.IndexOf(roundsUI[roundsUI.Count - roundsToFill]);
        
        for (int i = 0; i < roundsToFill; i++)
        {
            Image roundUI = roundsUI[startIndex + i];
                SetImageColor(roundUI, refillCurrentRoundColor);

            yield return roundInterval;
            
            if (i != roundsToFill -1)
                SetImageColor(roundUI, refillRoundColor);    

            if (i > (roundsToFill / 2))
            {
                StartCoroutine(FadeRefilledRounds(startIndex, roundsToFill));
            }
        }

        //yield return StartCoroutine(FadeRefilledRounds(startIndex, roundsToFill));
    }

    private IEnumerator FadeImageColor(Image image, Color target, float duration)
    {
        Color start = image.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            image.color = Color.Lerp(start, target, t);

            yield return null;
        }

        image.color = target;
    }

    private IEnumerator FadeRefilledRounds(int startIndex, int roundsToFill)
    {
        float fadeDurationPerRound = refillFadeDuration / roundsToFill;

        for (int i = 0; i < roundsToFill; i++)
        {
            int roundIndex = startIndex + i;
            Image roundUI = roundsUI[roundIndex];

            Color targetColor = GetImageTargetColor(roundIndex,roundsUI.Count);

            StartCoroutine(FadeImageColor(roundUI,targetColor, fadeDurationPerRound));

            yield return new WaitForSeconds(fadeDurationPerRound);
        }
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

    private Color GetImageTargetColor(int roundIndex, int availableRoundsAmount)
    {
        Color target;
        if (roundIndex == (availableRoundsAmount -1))
            target = currentRoundColor;
        else if (roundIndex < availableRoundsAmount)
            target = availableRoundColor;
        else
            target = unavailableRoundColor;

        return target;
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
