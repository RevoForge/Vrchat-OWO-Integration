using UdonSharp;
using UnityEngine;

public class AudioLinkRangeAmpGetter : UdonSharpBehaviour
{
    // For Use With AudioLink Package
    #if AUDIOLINK
    [Header("Main AudioLink Script")]
    [SerializeField]
    private AudioLink.AudioLink audioLink; // Use full namespace.class to avoid ambiguity
    [Header("Function Options")]
    [SerializeField] private bool watchBass = true;
    [SerializeField] private bool watchLowMid = true;
    [SerializeField] private bool watchHighMid = true;
    [SerializeField] private bool watchTrebble = true;
    [SerializeField] private bool sendDebugMSG = true;


    [HideInInspector] public float highestAmplitudeBass;
    [HideInInspector] public float frequencyOfHighestAmplitudeBass;
    [HideInInspector] public float highestAmplitudeLowMid;
    [HideInInspector] public float frequencyOfHighestAmplitudeLowMid;
    [HideInInspector] public float highestAmplitudeHighMid;
    [HideInInspector] public float frequencyOfHighestAmplitudeHighMid;
    [HideInInspector] public float highestAmplitudeTrebble;
    [HideInInspector] public float frequencyOfHighestAmplitudeTrebble;
    private float timer;
    [Header("Output Timer")]
    [SerializeField, Tooltip("Set to How Often you Want to See The Values")]
    private float timedelay = 0.1f;



    private void Update()
    {
    }
    private void LateUpdate()
    {
        if (audioLink != null)
        {
            timer += Time.deltaTime;
            if (timer > timedelay)
            {
                timer = 0f;
                if (watchBass)
                {
                    highestAmplitudeBass = 0;
                    frequencyOfHighestAmplitudeBass = 0;

                    for (float freq = 14f; freq <= 80; freq += 2f) 
                    {
                        Vector4 amp = audioLink.GetAmplitudeAtFrequency(freq);
                        if (amp.x > highestAmplitudeBass)
                        {
                            highestAmplitudeBass = amp.x;
                            frequencyOfHighestAmplitudeBass = freq;
                        }
                    }

                    if (highestAmplitudeBass >= 0.000001 && sendDebugMSG)
                    {
                        int convertedAmp = (int)(highestAmplitudeBass * 100);
                        Debug.Log($"The highest amplitude Of Bass is {convertedAmp} at {frequencyOfHighestAmplitudeBass}Hz.");
                    }
                }

                if (watchLowMid)
                {
                    highestAmplitudeLowMid = 0;
                    frequencyOfHighestAmplitudeLowMid = 0;

                    for (float freq = 80f; freq <= 440; freq += 5f) 
                    {
                        Vector4 amp = audioLink.GetAmplitudeAtFrequency(freq);

                        if (amp.x > highestAmplitudeLowMid)
                        {
                            highestAmplitudeLowMid = amp.x;
                            frequencyOfHighestAmplitudeLowMid = freq;
                        }
                    }
                    if (highestAmplitudeLowMid >= 0.000001 && sendDebugMSG)
                    {
                        int convertedAmp = (int)(highestAmplitudeLowMid * 100);
                        Debug.Log($"The highest amplitude Low Mid is {convertedAmp} at {frequencyOfHighestAmplitudeLowMid}Hz.");
                    }
                }

                if (watchHighMid)
                {
                    highestAmplitudeHighMid = 0;
                    frequencyOfHighestAmplitudeHighMid = 0;

                    for (float freq = 440f; freq <= 2440; freq += 50f) 
                    {
                        Vector4 amp = audioLink.GetAmplitudeAtFrequency(freq);

                        if (amp.x > highestAmplitudeHighMid)
                        {
                            highestAmplitudeHighMid = amp.x;
                            frequencyOfHighestAmplitudeHighMid = freq;
                        }
                    }
                    if (highestAmplitudeHighMid >= 0.000001 && sendDebugMSG)
                    {
                        int convertedAmp = (int)(highestAmplitudeHighMid * 100);
                        Debug.Log($"The highest amplitude High Mid is {convertedAmp} at {frequencyOfHighestAmplitudeHighMid}Hz.");
                    }
                }

                if (watchTrebble)
                {
                    highestAmplitudeTrebble = 0;
                    frequencyOfHighestAmplitudeTrebble = 0;

                    for (float freq = 2440f; freq <= 14040; freq += 100f) 
                    {
                        Vector4 amp = audioLink.GetAmplitudeAtFrequency(freq);

                        if (amp.x > highestAmplitudeTrebble)
                        {
                            highestAmplitudeTrebble = amp.x;
                            frequencyOfHighestAmplitudeTrebble = freq;
                        }
                    }
                    if (highestAmplitudeTrebble >= 0.000001 && sendDebugMSG)
                    {
                        int convertedAmp = (int)(highestAmplitudeTrebble * 100);
                        Debug.Log($"The highest amplitude Trebble is {convertedAmp} at {frequencyOfHighestAmplitudeTrebble}Hz.");

                    }
                }
            }
        }
    }
#else
    [Header("AudioLink Package Needs To Be Installed!")]
#endif
}
