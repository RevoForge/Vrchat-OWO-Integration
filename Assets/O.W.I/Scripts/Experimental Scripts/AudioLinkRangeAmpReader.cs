
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

public class AudioLinkRangeAmpReader : UdonSharpBehaviour
{
    //// For Use With AudioLink Package
#if AUDIOLINK
    [Header("Scene References")]
    public AudioLinkRangeAmpGetter audioLinkRangeAmpGetter;
    public OWIBassBoostedDucks boostedDucks;
    [Tooltip("Slider_Threshold0 on the AudioLinkController")]
    public Slider bassAmpSlider;
    [Tooltip("Slider_Threshold1 on the AudioLinkController")]
    public Slider lowMidSlider;
    [Tooltip("Slider_Threshold2 on the AudioLinkController")]
    public Slider highMidSlider;
    [Tooltip("Slider_Threshold3 on the AudioLinkController")]
    public Slider trebbleSlider;
    private float currentTimer1;
    private float currentTimer2;
    private float currentTimer3;
    private float currentTimer4;
    private float delayTimer1;
    private float delayTimer2;
    private float delayTimer3;
    private float delayTimer4;
    [Header("Function Choices")]
    [SerializeField] private bool watchBass = true;
    [SerializeField] private bool watchLowMid = true;
    [SerializeField] private bool watchHighMid = true;
    [SerializeField] private bool watchTrebble = true;
    [SerializeField] private bool sendDebugMSG = true;

    private void Start()
    {
        delayTimer1 = boostedDucks.bassDuration;
        delayTimer2 = boostedDucks.lowMidDuration;
        delayTimer3 = boostedDucks.highMidDuration;
        delayTimer4 = boostedDucks.trebbleDuration;
    }
    private void LateUpdate()
    {

        if (audioLinkRangeAmpGetter.highestAmplitudeBass >= bassAmpSlider.value && watchBass)
        {
            currentTimer1 += Time.deltaTime;
            if (currentTimer1 >= delayTimer1)
            {
                currentTimer1 = 0;
                boostedDucks.SendTheBassByIntensity(audioLinkRangeAmpGetter.highestAmplitudeBass);
                if (sendDebugMSG)
                {
                    Debug.Log("Sent Bass - Amplitude " + audioLinkRangeAmpGetter.highestAmplitudeBass + " Frequency " + audioLinkRangeAmpGetter.frequencyOfHighestAmplitudeBass);
                }

            }
        }
        if (audioLinkRangeAmpGetter.highestAmplitudeLowMid >= lowMidSlider.value && watchLowMid)
        {
            currentTimer2 += Time.deltaTime;
            if (currentTimer2 >= delayTimer2)
            {
                currentTimer2 = 0;
                boostedDucks.SendTheLowMidByIntensity(audioLinkRangeAmpGetter.highestAmplitudeLowMid);
                if (sendDebugMSG)
                {
                    Debug.Log("Sent Low Mid - Amplitude " + audioLinkRangeAmpGetter.highestAmplitudeLowMid + " Frequency " + audioLinkRangeAmpGetter.frequencyOfHighestAmplitudeLowMid);
                }

            }
        }
        if (audioLinkRangeAmpGetter.highestAmplitudeHighMid >= highMidSlider.value && watchHighMid)
        {
            currentTimer3 += Time.deltaTime;
            if (currentTimer3 >= delayTimer3)
            {
                currentTimer3 = 0;
                boostedDucks.SendTheHighMidByIntensity(audioLinkRangeAmpGetter.highestAmplitudeHighMid);
                if (sendDebugMSG)
                {
                    Debug.Log("Sent Low Mid - Amplitude " + audioLinkRangeAmpGetter.highestAmplitudeHighMid + " Frequency " + audioLinkRangeAmpGetter.frequencyOfHighestAmplitudeHighMid);
                }
            }
        }
        if (audioLinkRangeAmpGetter.highestAmplitudeTrebble >= trebbleSlider.value && watchTrebble)
        {
            currentTimer4 += Time.deltaTime;
            if (currentTimer4 >= delayTimer4)
            {
                currentTimer4 = 0;
                boostedDucks.SendTheTrebbleByIntensity(audioLinkRangeAmpGetter.highestAmplitudeTrebble);
                if (sendDebugMSG)
                {
                    Debug.Log("Sent Low Mid - Amplitude " + audioLinkRangeAmpGetter.highestAmplitudeTrebble + " Frequency " + audioLinkRangeAmpGetter.frequencyOfHighestAmplitudeTrebble);
                }
            }
        }
    }
#else
    [Header("Warning!!! AudioLink Package Needs To Be Installed!")]
#endif

}
