
using UdonSharp;
using UnityEngine;

public class OWIMicroSensationCreator : UdonSharpBehaviour
{
    // Muscle Collider Names
    private readonly string pectoralL = "owo_suit_Pectoral_L";
    private readonly string pectoralR = "owo_suit_Pectoral_R";
    private readonly string dorsalL = "owo_suit_Dorsal_L";
    private readonly string dorsalR = "owo_suit_Dorsal_R";
    private readonly string armL = "owo_suit_Arm_L";
    private readonly string armR = "owo_suit_Arm_R";
    private readonly string lumbarL = "owo_suit_Lumbar_L";
    private readonly string lumbarR = "owo_suit_Lumbar_R";
    private readonly string abdominalL = "owo_suit_Abdominal_L";
    private readonly string abdominalR = "owo_suit_Abdominal_R";
    // Muscle String Names
    private readonly string pectoralLm = "\"pectoral_L\": 100";
    private readonly string pectoralRm = "\"pectoral_R\": 100";
    private readonly string dorsalLm = "\"dorsal_L\": 100";
    private readonly string dorsalRm = "\"dorsal_R\": 100";
    private readonly string armLm = "\"arm_L\": 100";
    private readonly string armRm = "\"arm_R\": 100";
    private readonly string lumbarLm = "\"lumbar_L\": 100";
    private readonly string lumbarRm = "\"lumbar_R\": 100";
    private readonly string abdominalLm = "\"abdominal_L\": 100";
    private readonly string abdominalRm = "\"abdominal_R\": 100";
    private readonly string frontm = "\"frontMuscles\": 100";
    private readonly string backm = "\"backMuscles\": 100";
    private readonly string allm = "\"allMuscles\": 100";
    // Sensation String Parts
    private string start;
    private readonly string end = "}}]";
    // Inspector MicroSensation Setting
    [Header("MicroSensation Settings")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority = 1;
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName = "Default Name";
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

    // Logic Varibles
    private bool muscleTriggered = false;
    private string triggeredMuscles = "";
    private readonly float colliderCaptureTimer = 0.1f;
    private float currentTimer = 0f;
    private bool shouldProcess = false;
    [Header("Muscle Contact Overrides")]
    [SerializeField]
    private bool allMuscles;
    [SerializeField]
    private bool frontMuscles;
    [SerializeField]
    private bool backMuscles;

    private void Start()
    {
        start = $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"";
    }
    // Timer For Catching Multiple Muscle Trigger Events
    // Increase value to Catch For a Longer Time 
    private void Update()
    {
        if (shouldProcess)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= colliderCaptureTimer)
            {
                ProcessTriggeredZones();
                currentTimer = 0f;
                shouldProcess = false;
            }
        }
    }
    // Getting OWO World Avatar Collider Contact
    private void OnTriggerEnter(Collider other)
    {
        if (allMuscles && !triggeredMuscles.Contains(allm))
        {
            if (other.name == pectoralL
                || other.name == pectoralR
                || other.name == dorsalL
                || other.name == dorsalR
                || other.name == armL
                || other.name == armR
                || other.name == lumbarL
                || other.name == lumbarR
                || other.name == abdominalL
                || other.name == abdominalR)
            {
                triggeredMuscles += allm;
                muscleTriggered = true;
            }
            return;
        }
        if (frontMuscles && !triggeredMuscles.Contains(frontm))
        {
            if (other.name == pectoralL
                || other.name == pectoralR
                || other.name == dorsalL
                || other.name == dorsalR
                || other.name == armL
                || other.name == armR
                || other.name == lumbarL
                || other.name == lumbarR
                || other.name == abdominalL
                || other.name == abdominalR)
            {
                triggeredMuscles += frontm;
                muscleTriggered = true;
            }
        }
        if (backMuscles && !triggeredMuscles.Contains(backm))
        {
            if (other.name == pectoralL
                || other.name == pectoralR
                || other.name == dorsalL
                || other.name == dorsalR
                || other.name == armL
                || other.name == armR
                || other.name == lumbarL
                || other.name == lumbarR
                || other.name == abdominalL
                || other.name == abdominalR)
            {
                triggeredMuscles += frontMuscles ? "," + backm : backm;
                muscleTriggered = true;
            }
            if (frontMuscles) { return; }
        }
        else
        {
            if (!frontMuscles)
            {
                if (other.name == pectoralL && !triggeredMuscles.Contains(pectoralLm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + pectoralLm;
                    muscleTriggered = true;
                }
                if (other.name == pectoralR && !triggeredMuscles.Contains(pectoralRm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + pectoralRm;
                    muscleTriggered = true;
                }

                if (other.name == armL && !triggeredMuscles.Contains(armLm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + armLm;
                    muscleTriggered = true;
                }
                if (other.name == armR && !triggeredMuscles.Contains(armRm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + armRm;
                    muscleTriggered = true;

                }
                if (other.name == abdominalL && !triggeredMuscles.Contains(abdominalLm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + abdominalLm;
                    muscleTriggered = true;
                }
                if (other.name == abdominalR && !triggeredMuscles.Contains(abdominalRm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + abdominalRm;
                    muscleTriggered = true;
                }
            }
            if (!backMuscles)
            {
                if (other.name == dorsalL && !triggeredMuscles.Contains(dorsalLm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + dorsalLm;
                    muscleTriggered = true;
                }
                if (other.name == dorsalR && !triggeredMuscles.Contains(dorsalRm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + dorsalRm;
                    muscleTriggered = true;
                }
                if (other.name == lumbarL && !triggeredMuscles.Contains(lumbarLm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + lumbarLm;
                    muscleTriggered = true;
                }
                if (other.name == lumbarR && !triggeredMuscles.Contains(lumbarRm))
                {
                    triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + lumbarRm;
                    muscleTriggered = true;
                }
            }
        }

        if (muscleTriggered)
        {
            shouldProcess = true;
        }
    }
    // OWO MicroSensation StringBuilder
    private void ProcessTriggeredZones()
    {
        string builtString = start + sensationName + "\","
            + "\"frequency\": " + frequency + ","
            + "\"duration\": " + duration + ","
            + "\"intensity\": " + intensityPercentage + ","
            + "\"rampup\":" + rampUp + ","
            + "\"rampdown\":" + rampDown + ","
            + "\"exitdelay\":" + 0 + ","
            + "\"Muscles\": {" + triggeredMuscles
            + end;

        Debug.Log(builtString);

        triggeredMuscles = "";
        muscleTriggered = false;
    }
}
