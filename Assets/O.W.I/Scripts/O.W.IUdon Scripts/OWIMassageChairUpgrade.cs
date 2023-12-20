
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;


public class OWIMassageChairUpgrade : UdonSharpBehaviour
{
    private Animator animator;
    [SerializeField] private MassageExperience massageExperience;
    [Tooltip("Time In Seconds Between Massage Preset Restart \n Do Not set Lower than the Sum of ALL Durations In a Custom Massage \n Default Duration 0.4 ")]
    public float massageTimer = 0.5f;
    private string sensationName = "Massage";
    [Header("First Massage Pulse")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority;
    [SerializeField, Tooltip("Frequency for First Massage Pulse.")]
    [Range(1, 100)]
    private int frequency = 5;
    [SerializeField, Tooltip("Duration of First Massage Pulse.")]
    [Range(0.1f, 20)]
    private float duration = 0.1f;
    [SerializeField, Tooltip("Intensity percentage for First Massage Pulse.")]
    [Range(1, 100)]
    private int intensityPercentage = 25;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown = 0.1f;
    [SerializeField, Tooltip("Exit delay for the First Massage Pulse.")]
    [Range(0, 20)]
    private float exitDelay = 0f;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroupA;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroupB;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroupC;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroupD;
    [Header("Second Massage Pulse")]
    [SerializeField, Tooltip("Frequency for the Second Massage Pulse.")]
    [Range(1, 100)]
    private int frequency2 = 5;
    [SerializeField, Tooltip("Duration of the Second Massage Pulse.")]
    [Range(0.1f, 20)]
    private float duration2 = 0.1f;
    [SerializeField, Tooltip("Intensity percentage for the Second Massage Pulse.")]
    [Range(1, 100)]
    private int intensityPercentage2 = 25;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp2 = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown2 = 0.1f;
    [SerializeField, Tooltip("Exit delay for the Second Massage Pulse.")]
    [Range(0, 20)]
    private float exitDelay2 = 0f;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup2A;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup2B;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup2C;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup2D;
    [Header("Third Massage Pulse")]
    [SerializeField, Tooltip("Frequency for the Third Massage Pulse.")]
    [Range(1, 100)]
    private int frequency3 = 5;
    [SerializeField, Tooltip("Duration of the Third Massage Pulse.")]
    [Range(0.1f, 20)]
    private float duration3 = 0.1f;
    [SerializeField, Tooltip("Intensity percentage for the Third Massage Pulse.")]
    [Range(1, 100)]
    private int intensityPercentage3 = 25;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp3 = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown3 = 0.1f;
    [SerializeField, Tooltip("Exit delay for Third Massage Pulse.")]
    [Range(0, 20)]
    private float exitDelay3 = 0f;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup3A;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup3B;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup3C;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup3D;
    [Header("Forth Massage Pulse")]
    [SerializeField, Tooltip("Frequency for the Third Massage Pulse.")]
    [Range(1, 100)]
    private int frequency4 = 5;
    [SerializeField, Tooltip("Duration of the Third Massage Pulse.")]
    [Range(0.1f, 20)]
    private float duration4 = 0.1f;
    [SerializeField, Tooltip("Intensity percentage for the Third Massage Pulse.")]
    [Range(1, 100)]
    private int intensityPercentage4 = 25;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp4 = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown4 = 0.1f;
    [SerializeField, Tooltip("Exit delay for Third Massage Pulse.")]
    [Range(0, 20)]
    private float exitDelay4 = 0f;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup4A;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup4B;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup4C;
    [SerializeField, Tooltip("Muscle Groups to Trigger.")]
    private MuscleGroups muscleGroup4D;

    // Logic Variables
    private string start;
    private readonly string sensationNameStart = "\"sensation\": \"";
    private readonly string separator = "}},{";
    private readonly string end = "}}]";
    private bool[] massageStates = new bool[5];
    private Slider slider = null;
    private int intensity = 100;
    private string builtString1;
    private string builtMString1;
    private string builtString2;
    private string builtMString2;
    private string builtString3;
    private string builtMString3;
    private string builtString4;
    private string builtMString4;
    private string multiString;
    private bool turnedOn = false;
    private float currentTimer;

    private void Start()
    {
        start = $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},";
        animator = GetComponent<Animator>();
        slider = GetComponentInChildren<Slider>();
        massageStates[0] = true;

        builtMString1 = BuildMuscleString(muscleGroupA, muscleGroupB, muscleGroupC, muscleGroupD);
        builtMString2 = BuildMuscleString(muscleGroup2A, muscleGroup2B, muscleGroup2C, muscleGroup2D);
        builtMString3 = BuildMuscleString(muscleGroup3A, muscleGroup3B, muscleGroup3C, muscleGroup3D);
        builtMString4 = BuildMuscleString(muscleGroup4A, muscleGroup4B, muscleGroup4C, muscleGroup4D);
        builtString1 = BuildSensationString(sensationName, frequency, duration, intensityPercentage, rampUp, rampDown, exitDelay, builtMString1);
        builtString2 = BuildSensationString(sensationName, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, builtMString2);
        builtString3 = BuildSensationString(sensationName, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, builtMString3);
        builtString4 = BuildSensationString(sensationName, frequency4, duration4, intensityPercentage4, rampUp4, rampDown4, exitDelay4, builtMString4);
        multiString = BuildMultiSensationString(builtString1, builtString2, builtString3, builtString4);
    }

    private string GetMuscleString(MuscleGroups muscleGroup)
    {
        switch (muscleGroup)
        {
            case MuscleGroups.dorsal_L: return $"\"dorsal_L\":{intensity}";
            case MuscleGroups.dorsal_R: return $"\"dorsal_R\":{intensity}";
            case MuscleGroups.lumbar_L: return $"\"lumbar_L\":{intensity}";
            case MuscleGroups.lumbar_R: return $"\"lumbar_R\":{intensity}";
            case MuscleGroups.abdominal_L: return $"\"abdominal_L\":{intensity}";
            case MuscleGroups.abdominal_R: return $"\"abdominal_R\":{intensity}";
            case MuscleGroups.pectoral_L: return $"\"pectoral_L\":{intensity}";
            case MuscleGroups.pectoral_R: return $"\"pectoral_R\":{intensity}";
            default: return string.Empty;
        }
    }

    private string BuildMuscleString(MuscleGroups one, MuscleGroups two, MuscleGroups three, MuscleGroups four)
    {
        string muscle1 = GetMuscleString(one);
        string muscle2 = GetMuscleString(two);
        string muscle3 = GetMuscleString(three);
        string muscle4 = GetMuscleString(four);

        string result = "";

        if (!string.IsNullOrEmpty(muscle1)) result += muscle1;
        if (!string.IsNullOrEmpty(muscle2))
        {
            if (!string.IsNullOrEmpty(result)) result += ",";
            result += muscle2;
        }
        if (!string.IsNullOrEmpty(muscle3))
        {
            if (!string.IsNullOrEmpty(result)) result += ",";
            result += muscle3;
        }
        if (!string.IsNullOrEmpty(muscle4))
        {
            if (!string.IsNullOrEmpty(result)) result += ",";
            result += muscle4;
        }

        return result;
    }

    private string BuildSensationString(string sensation, int frequency, float durationVal, int intensityVal, float rampUp, float rampDown, float exitDelayVal, string muscles)
    {
        char[] charsToTrim = { ',' };
        if (string.IsNullOrEmpty(muscles))
        {
            return "";
        }
        return sensation + "\","
                + "\"frequency\": " + frequency + ","
                + "\"duration\": " + durationVal + ","
                + "\"intensity\": " + intensityVal + ","
                + "\"rampup\":" + rampUp + ","
                + "\"rampdown\":" + rampDown + ","
                + "\"exitdelay\":" + exitDelayVal + ","
                + "\"Muscles\": {" + muscles.TrimEnd(charsToTrim);
    }

    private string BuildMultiSensationString(string sensationOne, string sensationTwo, string sensationThree, string sensationFour)
    {
        string result = start;
        bool alreadyAdded = false;
        if (sensationOne.Length > 0)
        {
            result += sensationNameStart + sensationOne;
            alreadyAdded = true;
        }
        if (sensationTwo.Length > 0)
        {
            if (alreadyAdded) result += separator;
            result += sensationNameStart + sensationTwo;
            alreadyAdded = true;
        }
        if (sensationThree.Length > 0)
        {
            if (alreadyAdded) result += separator;
            result += sensationNameStart + sensationThree;
            alreadyAdded = true;
        }
        if (sensationFour.Length > 0)
        {
            if (alreadyAdded) result += separator;
            result += sensationNameStart + sensationFour;
        }
        if (result == start) return "STRING ERROR SET AT LEAST ONE MUSCLE GROUP";
        result += end;
        return result;
    }

    public override void Interact()
    {
        VRCPlayerApi interactingPlayer = Networking.LocalPlayer;

        if (interactingPlayer != null && interactingPlayer.isLocal)
        {
            interactingPlayer.UseAttachedStation();
        }
    }
    public override void OnStationEntered(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            turnedOn = true;
            if (massageStates[4])
            {
                animator.SetBool("SomeoneSeated", true);
            }
            massageExperience.OnMassageChairUse();
        }
    }
    public override void OnStationExited(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            turnedOn = false;
            if (massageStates[4])
            {
                animator.SetBool("SomeoneSeated", false);
            }
            massageExperience.OnMassageChairExit();
        }
    }

    private void Update()
    {
        if (turnedOn)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= massageTimer)
            {
                for (int i = 0; i < massageStates.Length; i++)
                {
                    if (massageStates[i])
                    {
                        Debug.Log($"{GetMessage(i)}");
                    }
                }
                currentTimer = 0f;
            }
        }
    }
    public void MassageType(int type)
    {
        for (int i = 0; i < massageStates.Length; i++)
        {
            massageStates[i] = false;
        }
        if (type >= 1 && type <= 5)
        {
            massageStates[type - 1] = true;
        }
    }
    public void SetIntensity()
    {
        intensity = (int)slider.value;
        builtMString1 = BuildMuscleString(muscleGroupA, muscleGroupB, muscleGroupC, muscleGroupD);
        builtMString2 = BuildMuscleString(muscleGroup2A, muscleGroup2B, muscleGroup2C, muscleGroup2D);
        builtMString3 = BuildMuscleString(muscleGroup3A, muscleGroup3B, muscleGroup3C, muscleGroup3D);
        builtMString4 = BuildMuscleString(muscleGroup4A, muscleGroup4B, muscleGroup4C, muscleGroup4D);
        builtString1 = BuildSensationString(sensationName, frequency, duration, intensityPercentage, rampUp, rampDown, exitDelay, builtMString1);
        builtString2 = BuildSensationString(sensationName, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, builtMString2);
        builtString3 = BuildSensationString(sensationName, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, builtMString3);
        builtString4 = BuildSensationString(sensationName, frequency4, duration4, intensityPercentage4, rampUp4, rampDown4, exitDelay4, builtMString4);
        multiString = BuildMultiSensationString(builtString1, builtString2, builtString3, builtString4);
    }

    private string GetMessage(int index)
    {
        switch (index)
        {
            case 0:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100,\"dorsal_R\": 100 }}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_L\": 100,\"lumbar_R\": 100 }}}}]";
            case 1:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100,\"lumbar_L\": 100 }}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{ \"dorsal_R\": 100,\"lumbar_R\": 100 }}}}]";
            case 2:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 15,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100,\"dorsal_R\": 100 }}}},{{ \"sensation\": \"Massage\",\"frequency\": 15,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_L\": 100,\"lumbar_R\": 100 }}}}]";
            case 3:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100}}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"dorsal_R\": 100}}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_L\": 100}}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_R\": 100}}}}]";
            case 4:
                return $"{multiString}";
            default:
                return "BUTTON TYPE ERROR";
        }
    }
}
