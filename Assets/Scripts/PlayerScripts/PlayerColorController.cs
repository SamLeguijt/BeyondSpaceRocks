using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorController : MonoBehaviour
{
    public delegate void PlayerColorHandler(ColorData colorData);
    public event PlayerColorHandler OnColorChanged;

    public ColorData CurrentColor { get; private set; }
    public int CurrentColorIndex { get; private set; }

    [Header("References")]
    [SerializeField] private SpriteRenderer PlayerSprite = null;
    [SerializeField] private PlayerInput inputController = null;

    public List<ColorData> AllColors { get; private set; } = new List<ColorData>();


    private void Start()
    {
        AllColors = GameManager.Instance.ColorContainer.GetAllColorData();
        CurrentColorIndex = 0;

        SwitchActiveColor(CurrentColorIndex);
    }

    private void OnEnable()
    {
        inputController.SwitchColorLeftInput += OnColorSwitchLeftInputReceivedEvent;
        inputController.SwitchColorRightInput += OnColorSwitchRightInputReceivedEvent;
    }

    private void OnDisable()
    {
        inputController.SwitchColorLeftInput -= OnColorSwitchLeftInputReceivedEvent;
        inputController.SwitchColorRightInput -= OnColorSwitchRightInputReceivedEvent;
    }

    private void OnColorSwitchLeftInputReceivedEvent()
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        SwitchActiveColor(-1);
    }

    private void OnColorSwitchRightInputReceivedEvent()
    {
        if (!GameManager.Instance.IsGameActive)
            return;
        
        SwitchActiveColor(1);
    }

    private void SwitchActiveColor(int direction)
    {


        direction = Mathf.Clamp(direction, -1, 1);

        int newColorIndex = CurrentColorIndex + direction;

        // Left
        if (direction < 0)
        {
            if (newColorIndex < 0)
            {
                newColorIndex = AllColors.Count - 1;
            }
        }
        // Right
        else if (direction > 0)
        {
            if (newColorIndex >= AllColors.Count)
            {
                newColorIndex = 0;
            }
        }


        AudioManager.Instance?.PlayColorSwitchSFX();

        CurrentColor = AllColors[newColorIndex];
        PlayerSprite.color = CurrentColor.Color;
        CurrentColorIndex = newColorIndex;

        OnColorChanged?.Invoke(CurrentColor);
    }

    public ColorData GetNextItem(List<ColorData> targetList, int index)
    {
        int newIndex = index + 1;

        if (newIndex >= targetList.Count)
        {
            newIndex = 0;
        }

        return targetList[newIndex];
    }

    public ColorData GetPreviousItem(List<ColorData> targetList, int index)
    {
        int newIndex = index - 1;

        if (newIndex < 0)
        {
            newIndex = targetList.Count -1;
        }

        return targetList[newIndex];
    }
}
