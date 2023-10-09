
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class OWIHandInteraction : UdonSharpBehaviour
{

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

    // Muscle String Intensity
    private int pectoralLmInt = 100;
    private int pectoralRmInt = 100;
    private int dorsalLmInt = 100;
    private int dorsalRmInt = 100;
    private int armLmInt = 100;
    private int armRmInt = 100;
    private int lumbarLmInt = 100;
    private int lumbarRmInt = 100;
    private int abdominalLmInt = 100;
    private int abdominalRmInt = 100;


    // String Parts
    private readonly string start = "VRC_OWO_WorldIntegration:[{";
    private readonly string sensationNameStart = "\"sensation\": \"";
    private readonly string sepperator = "}},{";
    private readonly string end = "}}]";
    [Header("First Zone Triggered Event")]
    [Header("Sensation Building Settings")]
    [SerializeField, Tooltip("Priority for overriding the previous Sensation event.")]
    private int sensationPriority = 1;
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName = "Finger Touch";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency = 20;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration = 0.2f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage = 20;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown = 0f;
    [SerializeField, Tooltip("Exit delay for the Second Sensation event.")]
    [Range(0, 20)]
    private float exitDelay = 0f;

    // Logic Varibles
    public float touchPunchThreshold = 0.01f;
    private bool muscleTriggered = false;
    private string triggeredMuscles = "";
    private string triggeredMuscles2 = "";
    private string triggeredMuscles3 = "";
    private readonly float delayTimer = 0.2f;
    private float currentTimer = 0f;
    private float triggerTimer = 0f;
    private bool shouldProcess = false;
    private int triggerCount = 0;
    private string builtString = "";
    private string builtString2 = "";
    private string builtString3 = "";
    private string multiString = "";
    private bool doubletrigger = false;
    private bool trippleTrigger = false;
    private bool triggered = false;
    private Vector3 originalTransformScale;
    private readonly float defaultScale = 1.9f;

    private int[] playerIds = new int[16];
    private bool[] isGameObjectActive = new bool[16];
    private float playerScale;
    private Vector3 _position;
    private bool punch = false;
    private VRCPlayerApi playerApi; // Reference to the player
    private HandType handType;
    private void OnEnable()
    {
        originalTransformScale = transform.localScale;
        _position = transform.position;
    }
    public void Initialize(VRCPlayerApi player, HandType type)
    {
        playerApi = player;
        handType = type;
    }
    private void CompareScalingDifference(float a, float b)
    {
        if (a == b)
        {
            return;
        }

        float difference = Mathf.Abs(a - b);
        float scaleMultiplier = a > b ? 1 - difference / a : 1 + difference / a;

        Vector3 newScale = originalTransformScale * scaleMultiplier;
        transform.localScale = newScale;
    }


    private void LateUpdate()
    {
        if (playerApi == null || !playerApi.IsValid()) return;
        if (playerScale != (float)Math.Round(playerApi.GetAvatarEyeHeightAsMeters(), 2))
        {
            playerScale = (float)Math.Round(playerApi.GetAvatarEyeHeightAsMeters(), 2);
            CompareScalingDifference(defaultScale, playerScale);
        }
        if (playerApi.IsUserInVR())
        {
            if (handType == HandType.Left)
            {
                transform.position = playerApi.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;
                transform.rotation = playerApi.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).rotation;
            }
            else if (handType == HandType.Right)
            {
                transform.position = playerApi.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;
                transform.rotation = playerApi.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).rotation;
            }
        }
        else
        {
            if (handType == HandType.Left)
            {
                transform.position = playerApi.GetBonePosition(HumanBodyBones.LeftHand);
                transform.rotation = playerApi.GetBoneRotation(HumanBodyBones.LeftHand);
            }
            else if (handType == HandType.Right)
            {
                transform.position = playerApi.GetBonePosition(HumanBodyBones.RightHand);
                transform.rotation = playerApi.GetBoneRotation(HumanBodyBones.RightHand);
            }
        }
    }


    private void Update()
    {
        var current = transform.position;
        var delta = Vector3.Distance(current, _position);
        var velocity = delta / Time.deltaTime;
        _position = current;
        if (velocity > touchPunchThreshold)
        {
            punch = true;
        }
        if (velocity <= touchPunchThreshold && punch)
        {
            punch = false;
        }
        if (shouldProcess)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= delayTimer && !triggered)
            {
                ProcessTriggeredZones();
                currentTimer = 0f;
                shouldProcess = false;
            }
        }
        if (triggered)
        {
            triggerTimer += Time.deltaTime;

            if (triggerTimer >= (trippleTrigger ? duration * 3 : (doubletrigger ? duration * 2 : duration)))
            {
                triggerTimer = 0f;
                shouldProcess = false;
                triggered = false;
                doubletrigger = false;
                trippleTrigger = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        string currentMuscle = "";

        switch (other.name)
        {
            case pectoralL:
                currentMuscle += pectoralLm + pectoralLmInt;
                muscleTriggered = true;
                break;
            case pectoralR:
                currentMuscle += pectoralRm + pectoralRmInt;
                muscleTriggered = true;
                break;
            case dorsalL:
                currentMuscle += dorsalLm + dorsalLmInt;
                muscleTriggered = true;
                break;
            case dorsalR:
                currentMuscle += dorsalRm + dorsalRmInt;
                muscleTriggered = true;
                break;
            case armL:
                currentMuscle += armLm + armLmInt;
                muscleTriggered = true;
                break;
            case armR:
                currentMuscle += armRm + armRmInt;
                muscleTriggered = true;
                break;
            case lumbarL:
                currentMuscle += lumbarLm + lumbarLmInt;
                muscleTriggered = true;
                break;
            case lumbarR:
                currentMuscle += lumbarRm + lumbarRmInt;
                muscleTriggered = true;
                break;
            case abdominalL:
                currentMuscle += abdominalLm + abdominalLmInt;
                muscleTriggered = true;
                break;
            case abdominalR:
                currentMuscle += abdominalRm + abdominalRmInt;
                muscleTriggered = true;
                break;
            default:
                return;
        }

        if (muscleTriggered)
        {
            if (triggerCount <= 3)
            {
                triggerCount++;
            }
            if (triggerCount == 1)
            {
                triggeredMuscles = currentMuscle;
            }
            if (triggerCount == 2)
            {
                triggeredMuscles2 = currentMuscle;
            }
            if (triggerCount == 3)
            {
                triggeredMuscles3 = currentMuscle;
            }
            shouldProcess = true;
            muscleTriggered = false;
        }
    }

    private void ProcessTriggeredZones()
    {
        builtString = "";
        builtString2 = "";
        builtString3 = "";
        if (triggerCount == 1)
        {
            int intensityPercentage2 = punch ? intensityPercentage * 3 : intensityPercentage;
            builtString = BuildSensationString(sensationName, frequency, duration, intensityPercentage2, rampUp, rampDown, exitDelay, triggeredMuscles);
            multiString = BuildMultiSensationString(builtString, builtString2, builtString3);
            Debug.Log(multiString);

        }
        if (triggerCount == 2)
        {
            int intensityPercentage2 = punch ? intensityPercentage * 3 : intensityPercentage;
            builtString = BuildSensationString(sensationName, frequency, duration, intensityPercentage2, rampUp, rampDown, exitDelay, triggeredMuscles);
            builtString2 = BuildSensationString(sensationName, frequency, duration, intensityPercentage2, rampUp, rampDown, exitDelay, triggeredMuscles2);
            multiString = BuildMultiSensationString(builtString, builtString2, builtString3);
            Debug.Log(multiString);
            doubletrigger = true;
        }
        if (triggerCount == 3)
        {
            int intensityPercentage2 = punch ? intensityPercentage * 3 : intensityPercentage;
            builtString = BuildSensationString(sensationName, frequency, duration, intensityPercentage2, rampUp, rampDown, exitDelay, triggeredMuscles);
            builtString2 = BuildSensationString(sensationName, frequency, duration, intensityPercentage2, rampUp, rampDown, exitDelay, triggeredMuscles2);
            builtString3 = BuildSensationString(sensationName, frequency, duration, intensityPercentage2, rampUp, rampDown, exitDelay, triggeredMuscles3);
            multiString = BuildMultiSensationString(builtString, builtString2, builtString3);
            Debug.Log(multiString);
            trippleTrigger = true;
        }
        triggered = true;
        triggerCount = 0;
    }

    private string BuildSensationString(string sensation, int frequency, float durationVal, int intensityVal, float rampUp, float rampDown, float exitDelayVal, string muscles)
    {
        return sensation + "\","
                + "\"frequency\": " + frequency + ","
                + "\"duration\": " + durationVal + ","
                + "\"intensity\": " + intensityVal + ","
                + "\"rampup\":" + rampUp + ","
                + "\"rampdown\":" + rampDown + ","
                + "\"exitdelay\":" + exitDelayVal + ","
                + "\"Muscles\": {" + muscles.TrimEnd(',');
    }
    private string BuildMultiSensationString(string sensationOne, string sensationTwo, string sensationThree)
    {
        if (sensationThree.Length > 0)
        {
            return start + "\"priority\":" + sensationPriority + "," + sensationNameStart + sensationOne + sepperator + sensationNameStart + sensationTwo + sepperator + sensationNameStart + sensationThree + end;
        }
        else if (sensationTwo.Length > 0)
        {
            return start + "\"priority\":" + sensationPriority + "," + sensationNameStart + sensationOne + sepperator + sensationNameStart + sensationTwo + end;
        }
        else if (sensationOne.Length > 0)
        {
            return start + "\"priority\":" + sensationPriority + "," + sensationNameStart + sensationOne + end;
        }
        return "ERROR";
    }
}
