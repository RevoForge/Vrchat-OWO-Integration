using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderManagerUdon : UdonSharpBehaviour
{
    [Header("Text To Update")]
    public TextMeshProUGUI valueText;

    [Header("Settings")]
    public bool usePercent = false;
    public bool showValue = true;
    public bool useRoundValue = false;
    public bool useMillaSecond = false;
    public bool useFloatThird = false;

    private Slider mainSlider;

    void Start()
    {
        mainSlider = GetComponent<Slider>();
        if (!showValue)
            valueText.enabled = false;
        UpdateValueText();
    }

    public void OnSliderValueChanged() // This function should be set up in Unity to listen to the slider's change event.
    {
        UpdateValueText();
    }

    private void UpdateValueText()
    {
        string valueSuffix = ""; // Default suffix
        float displayValue = mainSlider.value;

        if (useRoundValue)
        {
            displayValue = Mathf.Round(mainSlider.value);
            valueSuffix = usePercent ? "%" : valueSuffix;
        }
        else if (useFloatThird)
        {
            valueSuffix = usePercent ? "%" : valueSuffix;
            displayValue = float.Parse(mainSlider.value.ToString("F3"));
        }
        else
        {
            if (useMillaSecond)
            {
                displayValue = mainSlider.value * 1000f;
                displayValue = Mathf.Round(displayValue / 100f) * 100f;
                valueSuffix = "ms";
            }
            else
            {
                valueSuffix = usePercent ? "%" : valueSuffix;
                displayValue = float.Parse(mainSlider.value.ToString("F1"));
            }
        }

        valueText.text = $"{displayValue}{valueSuffix}";
    }
}
