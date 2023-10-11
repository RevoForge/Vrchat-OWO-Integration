
using UdonSharp;
using UnityEngine;

public class OWIGlobalSensation : UdonSharpBehaviour
{
    private float currentTimer;
    
    private readonly string[] musclesArray = {
        "\"pectoral_L\": 100", "\"pectoral_R\": 100", "\"dorsal_L\": 100", "\"dorsal_R\": 100", "\"arm_L\": 100",
        "\"arm_R\": 100", "\"lumbar_L\": 100", "\"lumbar_R\": 100", "\"abdominal_L\": 100", "\"abdominal_R\": 100"
    };
    
    [Header("Enable to Play Sensation on World Load")]
    [SerializeField] private bool startSensationOnLoad = false;

    [Header("MicroSensation Settings")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority = 1;
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName = "One";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration = 0.1f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage = 100;
    [SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp = 0f;
    [SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown = 0f;

    [Header("Time Between replay")]
    [SerializeField, Range(0.1f,20)]private float delayTimer = 0.1f;

    [Header("Muscle Choices")]
    [SerializeField, Tooltip("Ignores Other Muscle Choices sets a Random Muscle for each time the sensation Repeats.")] private bool UseRandomMuscle;

    [Header("Ignored If Use Random Muscle is True")]
    [SerializeField] private bool UseMusclePectoralLeft;
    [SerializeField] private bool UseMusclePectoralRight;
    [SerializeField] private bool UseMuscleDorsalLeft;
    [SerializeField] private bool UseMuscleDorsalRight;
    [SerializeField] private bool UseMuscleArmLeft;
    [SerializeField] private bool UseMuscleArmRight;
    [SerializeField] private bool UseMuscleLumbarLeft;
    [SerializeField] private bool UseMuscleLumbarRight;
    [SerializeField] private bool UseMuscleAbdominalLeft;
    [SerializeField] private bool UseMuscleAbdominalRight;
    private string builtMuscles = "";

    private void Start()
    {
        currentTimer = duration;
       // delayTimer = duration;
        if (!UseRandomMuscle)
        {
            if (UseMusclePectoralLeft)
            {
                builtMuscles += musclesArray[0] + ",";
            }
            if (UseMusclePectoralRight)
            {
                builtMuscles += musclesArray[1] + ",";
            }
            if (UseMuscleDorsalLeft)
            {
                builtMuscles += musclesArray[2] + ",";
            }
            if (UseMuscleDorsalRight)
            {
                builtMuscles += musclesArray[3] + ",";
            }
            if (UseMuscleArmLeft)
            {
                builtMuscles += musclesArray[4] + ",";
            }
            if (UseMuscleArmRight)
            {
                builtMuscles += musclesArray[5] + ",";
            }
            if (UseMuscleLumbarLeft)
            {
                builtMuscles += musclesArray[6] + ",";
            }
            if (UseMuscleLumbarRight)
            {
                builtMuscles += musclesArray[7] + ",";
            }
            if (UseMuscleAbdominalLeft)
            {
                builtMuscles += musclesArray[8] + ",";
            }
            if (UseMuscleAbdominalRight)
            {
                builtMuscles += musclesArray[9];
            }
            builtMuscles = builtMuscles.TrimEnd(',');
        }
    }
    private void Update()
    {
        if (UseRandomMuscle)
        {
            if (currentTimer >= delayTimer - 0.01f)
            {
                builtMuscles = GetRandomMuscle();
            }
        }
    }
    public void SetSensationParameters(int freq,float dur ,int inten,float rup,float rdwm)
    {
        frequency = freq;
        duration = dur;
        intensityPercentage = inten;
        rampUp = rup;
        rampDown = rdwm;
    }
    public void StartSensation()
    {
        startSensationOnLoad = true;
    }
    public void StopSensation()
    {
        startSensationOnLoad = false;
    }
    private string GetRandomMuscle()
    {
        int randomIndex = Random.Range(0, musclesArray.Length);
        return musclesArray[randomIndex];
    }
    private void LateUpdate()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= delayTimer & startSensationOnLoad)
        {
            Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"{sensationName}\",\"frequency\": {frequency},\"duration\": {duration},\"intensity\": {intensityPercentage},\"rampup\":{rampUp},\"rampdown\":{rampDown},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
            currentTimer = 0f;
        }
    }
}
