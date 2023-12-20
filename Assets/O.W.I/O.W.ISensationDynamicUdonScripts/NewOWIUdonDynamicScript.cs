using UdonSharp;
using UnityEngine;

public class NewOWIUdonDynamicScript : UdonSharpBehaviour
{
    [Header("Events")]
    // Muscle Collider Names
    private const string pectoralL = "owo_suit_Pectoral_L";
    private const string pectoralR = "owo_suit_Pectoral_R";
    private const string dorsalL = "owo_suit_Dorsal_L";
    private const string dorsalR = "owo_suit_Dorsal_R";
    private const string armL = "owo_suit_Arm_L";
    private const string armR = "owo_suit_Arm_R";
    private const string lumbarL = "owo_suit_Lumbar_L";
    private const string lumbarR = "owo_suit_Lumbar_R";
    private const string abdominalL = "owo_suit_Abdominal_L";
    private const string abdominalR = "owo_suit_Abdominal_R";
    // Muscle String Names
    private readonly string pectoralLm = "\"pectoral_L\":";
    private readonly string pectoralRm = "\"pectoral_R\":";
    private readonly string dorsalLm = "\"dorsal_L\":";
    private readonly string dorsalRm = "\"dorsal_R\":";
    private readonly string armLm = "\"arm_L\":";
    private readonly string armRm = "\"arm_R\":";
    private readonly string lumbarLm = "\"lumbar_L\":";
    private readonly string lumbarRm = "\"lumbar_R\":";
    private readonly string abdominalLm = "\"abdominal_L\":";
    private readonly string abdominalRm = "\"abdominal_R\":";
    [SerializeField, Tooltip("Enable to Stop the Last Triggered Sensation From Playing on All Muscles")] private bool ignoreGroupedMuscleSensation = false;
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")] private int sensationPriority = 1;
    [SerializeField] private float colliderCaptureTimer = 0.1f;
    // Muscle Intensity Ovverrides
    private readonly int[] pectoralLmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] pectoralRmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] dorsalLmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] dorsalRmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] armLmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] armRmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] lumbarLmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] lumbarRmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] abdominalLmIntValues = { 100, 100, 100, 100, 100 };
    private readonly int[] abdominalRmIntValues = { 100, 100, 100, 100, 100 };
    private readonly string start = "VRC_OWO_WorldIntegration:[";
    private readonly string sensationNameStart = "\"sensation\": \"";
    private readonly string separator = "}},{";
    private readonly string end = "}}]";
    private string[] triggerMusclesString = { "", "", "", "", "", "", "", "", "", "", "" };
    // Logic Variables
    private float currentTimer = 0f;
    private float triggerTimer = 0f;
    private bool shouldProcess = false;
    private int triggerCount = 0;
    private string builtString1 = "";
    private string builtString2 = "";
    private string builtString3 = "";
    private string builtString4 = "";
    private string builtString5 = "";
    private string multiString = "";
    private float builtdurations = 0f;
    private bool triggered = false;

    [Header("Event 1")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName1 = "First";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency1 = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration1 = 1f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage1 = 50;
    [SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp1 = 0f;
    [SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown1 = 0f;
    [SerializeField, Tooltip("Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay1 = 0f;

    [Header("Event 2")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName2 = "Second";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency2 = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration2 = 1f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage2 = 50;
    [SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp2 = 0f;
    [SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown2 = 0f;
    [SerializeField, Tooltip("Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay2 = 0f;

    [Header("Event 3")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName3 = "Third";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency3 = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration3 = 1f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage3 = 50;
    [SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp3 = 0f;
    [SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown3 = 0f;
    [SerializeField, Tooltip("Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay3 = 0f;

    [Header("Event 4")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName4 = "Fourth";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency4 = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration4 = 1f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage4 = 50;
    [SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp4 = 0f;
    [SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown4 = 0f;
    [SerializeField, Tooltip("Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay4 = 0f;

    [Header("Event 5")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName5 = "Fifth";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency5 = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration5 = 1f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage5 = 50;
    [SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp5 = 0f;
    [SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown5 = 0f;
    [SerializeField, Tooltip("Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay5 = 0f;


    private void Start()
    {
        builtdurations += duration1;
        builtdurations += duration2;
        builtdurations += duration3;
        builtdurations += duration4;
        builtdurations += duration5;
    }

    private void Update()
    {
        if (shouldProcess)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= colliderCaptureTimer && !triggered)
            {
                ProcessTriggeredZones();
                currentTimer = 0f;
                shouldProcess = false;
            }
        }
        if (triggered)
        {
            triggerTimer += Time.deltaTime;
            if (triggerTimer >= builtdurations)
            {
                triggerTimer = 0f;
                shouldProcess = false;
                triggered = false;
                triggerCount = 0;
                triggerMusclesString = new string[] { "", "", "", "", "", "", "", "", "", "" };
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerCount < 5)
        {
            string currentMuscle = "";
            switch (other.name)
            {
                case pectoralL:
                    currentMuscle += pectoralLm + pectoralLmIntValues[triggerCount];
                    break;
                case pectoralR:
                    currentMuscle += pectoralRm + pectoralRmIntValues[triggerCount];
                    break;
                case dorsalL:
                    currentMuscle += dorsalLm + dorsalLmIntValues[triggerCount];
                    break;
                case dorsalR:
                    currentMuscle += dorsalRm + dorsalRmIntValues[triggerCount];
                    break;
                case armL:
                    currentMuscle += armLm + armLmIntValues[triggerCount];
                    break;
                case armR:
                    currentMuscle += armRm + armRmIntValues[triggerCount];
                    break;
                case lumbarL:
                    currentMuscle += lumbarLm + lumbarLmIntValues[triggerCount];
                    break;
                case lumbarR:
                    currentMuscle += lumbarRm + lumbarRmIntValues[triggerCount];
                    break;
                case abdominalL:
                    currentMuscle += abdominalLm + abdominalLmIntValues[triggerCount];
                    break;
                case abdominalR:
                    currentMuscle += abdominalRm + abdominalRmIntValues[triggerCount];
                    break;
                default:
                    return;
            }
            if (!string.IsNullOrEmpty(currentMuscle))
            {
                triggerCount++;
                shouldProcess = true;
            }
            if (triggerCount == 1) { triggerMusclesString[0] = currentMuscle; }
            if (triggerCount == 2) { triggerMusclesString[1] = currentMuscle; }
            if (triggerCount == 3) { triggerMusclesString[2] = currentMuscle; }
            if (triggerCount == 4) { triggerMusclesString[3] = currentMuscle; }
            if (triggerCount == 5) { triggerMusclesString[4] = currentMuscle; }
        }

    }

    private void ProcessTriggeredZones()
    {
        triggered = true;
        if (triggerCount == 1)
        {
            if (!ignoreGroupedMuscleSensation)
            {
                builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[0]);
            }
            else
            {
                builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[0]);
            }
        }
        if (triggerCount == 2)
        {
            builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[0]);
            if (!ignoreGroupedMuscleSensation)
            {
                builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[0] + "," + triggerMusclesString[1]);
            }
            else
            {
                builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[1]);
            }
        }
        if (triggerCount == 3)
        {
            builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[0]);
            builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[1]);
            if (!ignoreGroupedMuscleSensation)
            {
                builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggerMusclesString[0] + "," + triggerMusclesString[1] + "," + triggerMusclesString[2]);
            }
            else
            {
                builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggerMusclesString[2]);
            }
        }
        if (triggerCount == 4)
        {
            builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[0]);
            builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[1]);
            builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggerMusclesString[2]);
            if (!ignoreGroupedMuscleSensation)
            {
                builtString4 = BuildSensationString(sensationName4, frequency4, duration4, intensityPercentage4, rampUp4, rampDown4, exitDelay4, triggerMusclesString[0] + "," + triggerMusclesString[1] + "," + triggerMusclesString[2] + "," + triggerMusclesString[3]);
            }
            else
            {
                builtString4 = BuildSensationString(sensationName4, frequency4, duration4, intensityPercentage4, rampUp4, rampDown4, exitDelay4, triggerMusclesString[3]);
            }
        }
        if (triggerCount == 5)
        {
            builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[0]);
            builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[1]);
            builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggerMusclesString[2]);
            builtString4 = BuildSensationString(sensationName4, frequency4, duration4, intensityPercentage4, rampUp4, rampDown4, exitDelay4, triggerMusclesString[3]);
            if (!ignoreGroupedMuscleSensation)
            {
                builtString5 = BuildSensationString(sensationName5, frequency5, duration5, intensityPercentage5, rampUp5, rampDown5, exitDelay5, triggerMusclesString[0] + "," + triggerMusclesString[1] + "," + triggerMusclesString[2] + "," + triggerMusclesString[3] + "," + triggerMusclesString[4]);
            }
            else
            {
                builtString5 = BuildSensationString(sensationName5, frequency5, duration5, intensityPercentage5, rampUp5, rampDown5, exitDelay5, triggerMusclesString[4]);
            }
        }
        multiString = BuildMultiSensationString(builtString1, builtString2, builtString3, builtString4, builtString5, triggerCount);
        Debug.Log(multiString);
        builtString1 = ""; builtString2 = ""; builtString3 = ""; builtString4 = ""; builtString5 = "";
        triggerMusclesString = new string[] { "", "", "", "", "", "", "", "", "", "" };

    }

    private string BuildSensationString(string sensation, int frequency, float durationVal, int intensityVal, float rampUp, float rampDown, float exitDelayVal, string muscles)
    {
        char[] charsToTrim = { ',' };
        return sensation + "\","
        + "\"frequency\": " + frequency + ","
        + "\"duration\": " + durationVal + ","
        + "\"intensity\": " + intensityVal + ","
        + "\"rampup\":" + rampUp + ","
        + "\"rampdown\":" + rampDown + ","
        + "\"exitdelay\":" + exitDelayVal + ","
        + "\"Muscles\": {" + muscles.TrimEnd(charsToTrim);

    }

    private string BuildMultiSensationString(string builtString1, string builtString2, string builtString3, string builtString4, string builtString5, int numberOfStrings)
    {
        if (numberOfStrings == 1)
        {
            return start + "{\"priority\":" + sensationPriority + "," + sensationNameStart + builtString1 + end;
        }
        if (numberOfStrings == 2)
        {
            return start + "{\"priority\":" + sensationPriority + "," + sensationNameStart + builtString1 + separator + sensationNameStart + builtString2 + end;
        }
        if (numberOfStrings == 3)
        {
            return start + "{\"priority\":" + sensationPriority + "," + sensationNameStart + builtString1 + separator + sensationNameStart + builtString2 + separator + sensationNameStart + builtString3 + end;
        }
        if (numberOfStrings == 4)
        {
            return start + "{\"priority\":" + sensationPriority + "," + sensationNameStart + builtString1 + separator + sensationNameStart + builtString2 + separator + sensationNameStart + builtString3 + separator + sensationNameStart + builtString4 + end;
        }
        if (numberOfStrings == 5)
        {
            return start + "{\"priority\":" + sensationPriority + "," + sensationNameStart + builtString1 + separator + sensationNameStart + builtString2 + separator + sensationNameStart + builtString3 + separator + sensationNameStart + builtString4 + separator + sensationNameStart + builtString5 + end;
        }
        return "Number of Trigger Counts ERROR";

    }
}
