using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public float fontSizeIncrease = 1f; // Amount to increase font size by
    private TextMeshProUGUI text; // The TextMeshPro component
    private GameObject graphic; // The background graphic
    private float originalFontSize; // To store the original font size
    private FontStyles originalFontStyle; // To store the original font style

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        graphic = transform.Find("BackgroundGraphic")?.gameObject;

        if (text != null)
        {
            originalFontSize = text.fontSize;
            originalFontStyle = text.fontStyle;
        }

        if (graphic != null)
        {
            graphic.SetActive(false); // Hide graphic initially
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowHoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideHoverEffect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowHoverEffect();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideHoverEffect();
    }

    private void ShowHoverEffect()
    {
        if (graphic != null)
        {
            graphic.SetActive(true); // Show graphic
        }

        if (text != null)
        {
            text.fontSize = originalFontSize + fontSizeIncrease; // Increase font size
            text.fontStyle = FontStyles.Bold; // Set text to bold
        }
    }

    private void HideHoverEffect()
    {
        if (graphic != null)
        {
            graphic.SetActive(false); // Hide graphic
        }

        if (text != null)
        {
            text.fontSize = originalFontSize; // HighscoreManagerEditor to original font size
            text.fontStyle = originalFontStyle; // HighscoreManagerEditor to original font style
        }
    }
}

