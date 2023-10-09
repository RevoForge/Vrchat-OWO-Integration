
using UdonSharp;
using UnityEngine;

public class OWIBassBoostedDucks : UdonSharpBehaviour
{
    private readonly string[] musclesArray = {
        "\"pectoral_L\": 100", "\"pectoral_R\": 100", "\"dorsal_L\": 100", "\"dorsal_R\": 100", "\"arm_L\": 100",
        "\"arm_R\": 100", "\"lumbar_L\": 100", "\"lumbar_R\": 100", "\"abdominal_L\": 100", "\"abdominal_R\": 100"
    };
    private string builtMuscles = "";


    [Header("Bass Hit")]
    [Header("MicroSensation Settings")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int bassPriority = 1;
    [Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    public int bassFrequency = 100;
    [Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    public float bassDuration = 0.2f;
    [Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    public int bassIntensity = 100;
    [Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float bassRampUp = 0f;
    [Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float bassRampDown = 0f;
    [Header("Low Mid Hit")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int lowMidPriority = 1;
    [Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    public int lowMidFrequency = 100;
    [Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    public float lowMidDuration = 0.2f;
    [Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    public int lowMidIntensity = 100;
    [Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float lowMidRampUp = 0f;
    [Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float lowMidRampDown = 0f;
    [Header("High Mid Hit")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int highMidPriority = 1;
    [Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    public int highMidFrequency = 100;
    [Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    public float highMidDuration = 0.2f;
    [Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    public int highMidIntensity = 100;
    [Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float highMidRampUp = 0f;
    [Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float highMidRampDown = 0f;
    [Header("Trebble Hit")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int trebblePriority = 1;
    [Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    public int trebbleFrequency = 100;
    [Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    public float trebbleDuration = 0.2f;
    [Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    public int trebbleIntensity = 100;
    [Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float trebbleRampUp = 0f;
    [Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    public float trebbleRampDown = 0f;

    private void Start()
    {
                builtMuscles += musclesArray[0] + ",";
                builtMuscles += musclesArray[1] + ",";
                builtMuscles += musclesArray[2] + ",";
                builtMuscles += musclesArray[3] + ",";
                builtMuscles += musclesArray[4] + ",";
                builtMuscles += musclesArray[5] + ",";
                builtMuscles += musclesArray[6] + ",";
                builtMuscles += musclesArray[7] + ",";
                builtMuscles += musclesArray[8] + ",";
                builtMuscles += musclesArray[9];
    }
    public void SetBassParameters(int freq, float dur, int inten, float rup, float rdwm)
    {
        bassFrequency = freq;
        bassDuration = dur;
        bassIntensity = inten;
        bassRampUp = rup;
        bassRampDown = rdwm;
    }
    public void SendTheBass()
    {
            Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {bassPriority},\"sensation\": \"Bass Hit\",\"frequency\": {bassFrequency},\"duration\": {bassDuration},\"intensity\": {bassIntensity},\"rampup\":{bassRampUp},\"rampdown\":{bassRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }

    public void SendTheBassByIntensity(float bassHit)
    {
        //Expected Hit Value 0 to 1
        int bassHits = (int)(bassHit * 100);
        Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {bassPriority},\"sensation\": \"Bass Hit\",\"frequency\": {bassFrequency},\"duration\": {bassDuration},\"intensity\": {bassHits},\"rampup\":{bassRampUp},\"rampdown\":{bassRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }
    public void SendTheLowMid()
    {
        Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {lowMidPriority},\"sensation\": \"Low Mid Hit\",\"frequency\": {lowMidFrequency},\"duration\": {lowMidDuration},\"intensity\": {lowMidIntensity},\"rampup\":{lowMidRampUp},\"rampdown\":{lowMidRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }
    public void SendTheLowMidByIntensity(float lowMidHit)
    {
        //Expected Hit Value 0 to 1
        int lowMidHits = (int)(lowMidHit * 100);
        Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {lowMidPriority},\"sensation\": \"Low Mid Hit\",\"frequency\": {lowMidFrequency},\"duration\": {lowMidDuration},\"intensity\": {lowMidHits},\"rampup\":{lowMidRampUp},\"rampdown\":{lowMidRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }
    public void SendTheHighMid()
    {
        Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {highMidPriority},\"sensation\": \"High Mid Hit\",\"frequency\": {highMidFrequency},\"duration\": {highMidDuration},\"intensity\": {highMidIntensity},\"rampup\":{highMidRampUp},\"rampdown\":{highMidRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }
    public void SendTheHighMidByIntensity(float highMidHit)
    {
        //Expected Hit Value 0 to 1
        int highMidHits = (int)(highMidHit * 100);
        Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {highMidPriority},\"sensation\": \"High Mid Hit\",\"frequency\": {highMidFrequency},\"duration\": {highMidDuration},\"intensity\": {highMidHits},\"rampup\":{highMidRampUp},\"rampdown\":{highMidRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }
    public void SendTheTrebble()
    {
        Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {trebblePriority},\"sensation\": \"Trebble Hit\",\"frequency\": {trebbleFrequency},\"duration\": {trebbleDuration},\"intensity\": {trebbleIntensity},\"rampup\":{trebbleRampUp},\"rampdown\":{trebbleRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }
    public void SendTheTrebbleByIntensity(float trebbleHit)
    {
        //Expected Hit Value 0 to 1
        int trebbleHits = (int)(trebbleHit * 100);
        Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {trebblePriority},\"sensation\": \"Trebble Hit\",\"frequency\": {trebbleFrequency},\"duration\": {trebbleDuration},\"intensity\": {trebbleHits},\"rampup\":{trebbleRampUp},\"rampdown\":{trebbleRampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
    }
}
