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

    [SerializeField, Range(0,1)] private float animationRefillFadeSplit = 0.5f;

    private Coroutine reloadAnimationCoroutine = null;
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
        if (reloadAnimationCoroutine != null)
            StopCoroutine(reloadAnimationCoroutine);

        int refillAmount = weapon.WeaponData.MaxAmmo - weapon.CurrentAmmo;
       
        AnimateReload(refillAmount, weapon.WeaponData.ReloadSeconds);
    }

    private void OnWeaponReloadFinishEvent(AbstractWeapon weapon)
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

    private void AnimateReload(int amountReloaded, float maxDuration)
    {
        if (reloadAnimationCoroutine != null)
            StopCoroutine(reloadAnimationCoroutine);

        reloadAnimationCoroutine = StartCoroutine(AnimationCoroutine(amountReloaded, maxDuration));
    }

    private IEnumerator AnimationCoroutine(int amountReloaded, float maxDuration)
    {
        float refullPhaseDuration = maxDuration * animationRefillFadeSplit;
        float fadeToDefaultPhaseDuration = maxDuration - refullPhaseDuration;

        yield return StartCoroutine(RefillAmmoRoundsRoutine(amountReloaded, refullPhaseDuration));
        
        StartCoroutine(FadeRoundsToDefaultColorRoutine(amountReloaded, fadeToDefaultPhaseDuration));
    }

    private IEnumerator FadeRoundsToDefaultColorRoutine(int roundsAmount, float maxDuration)
    {
        float roundDelay = maxDuration / roundsAmount;
        WaitForSeconds roundInterval = new WaitForSeconds(maxDuration / roundsAmount);
        Debug.Log("Default fade start, duration: " + maxDuration);

        int startIndex = roundsUI.IndexOf(roundsUI[roundsUI.Count - roundsAmount]);
        
        for (int i = 0; i < roundsAmount; i++)
        {
            int currentRoundIndex = startIndex + i;
            Image roundUI = roundsUI[currentRoundIndex];

            StartCoroutine(ImageColorFade(roundUI, availableRoundColor, roundDelay));
            yield return roundInterval;

            if (i == roundsAmount -1)
                SetImageColor(roundUI, currentRoundColor);  
        }

        reloadAnimationCoroutine = null;
    }

    private IEnumerator RefillAmmoRoundsRoutine(int roundsAmount, float maxDuration)
    {
        Debug.Log("Refill start, duration: " + maxDuration);
        WaitForSeconds roundInterval = new WaitForSeconds(maxDuration / roundsAmount);

        int startIndex = roundsUI.IndexOf(roundsUI[roundsUI.Count - roundsAmount]);
        
        for (int i = 0; i < roundsAmount; i++)
        {
            Image roundUI = roundsUI[startIndex + i];
            SetImageColor(roundUI, refillCurrentRoundColor);

            yield return roundInterval;
            
            if (i != roundsAmount -1)
                SetImageColor(roundUI, refillRoundColor);    
        }
    }

    private IEnumerator ImageColorFade(Image image, Color target, float fadeDuration)
    {
        Color start = image.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / fadeDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            image.color = Color.Lerp(start, target, t);

            yield return null;
        }

        image.color = target;
    }


    private void UpdateDisplayText(int availableRoundsAmount, int maxRoundsAmount)
    {
        string result;

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

    private void SetImageColor(Image image, Color target, float fadeDuration = 0.2f)
    {
        if (image.color == target)
            return;
        
        if (Mathf.Approximately(fadeDuration, 0))
            image.color = target; 
        else
            StartCoroutine(ImageColorFade(image, target, fadeDuration));
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
