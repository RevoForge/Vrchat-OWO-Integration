
using UdonSharp;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class OWIBullet : UdonSharpBehaviour
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
    private readonly string pectoralLm = "\"pectoral_L\":100";
    private readonly string pectoralRm = "\"pectoral_R\":100";
    private readonly string dorsalLm = "\"dorsal_L\":100";
    private readonly string dorsalRm = "\"dorsal_R\":100";
    private readonly string armLm = "\"arm_L\":100";
    private readonly string armRm = "\"arm_R\":100";
    private readonly string lumbarLm = "\"lumbar_L\":100";
    private readonly string lumbarRm = "\"lumbar_R\":100";
    private readonly string abdominalLm = "\"abdominal_L\":100";
    private readonly string abdominalRm = "\"abdominal_R\":100";

    [SerializeField]
    private float bulletSpeed = 10f;
    // String Parts
    private readonly string start = "VRC_OWO_WorldIntegration:[{";
    private string sensationNameStart = "\"sensation\": \"";
    private readonly string separator = "}},{";
    private readonly string end = "}}]";
    [Header("First Zone Triggered Event")]
    [Header("Sensation Building Settings")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority = 1;
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName = "Entry";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency = 30;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration = 0.5f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage = 100;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown = 0f;
    [SerializeField, Tooltip("Exit delay for the  Sensation event.")]
    [Range(0, 20)]
    private float exitDelay = 0f;
    [Header("ary Zone Triggered Event")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName2 = "Exit";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency2 = 30;
    [SerializeField, Tooltip("Duration of the  Sensation event.")]
    [Range(0.1f, 20)]
    private float duration2 = 0.5f;
    [SerializeField, Tooltip("Intensity percentage for the  Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage2 = 100;
    [SerializeField, Tooltip(" Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp2 = 0f;
    [SerializeField, Tooltip(" Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown2 = 0f;
    [SerializeField, Tooltip(" Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay2 = 0f;
    [Header("Muscle Group After Event")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName3 = "Bleeding";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency3 = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration3 = 5f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage3 = 50;
    [SerializeField, Tooltip(" Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp3 = 0f;
    [SerializeField, Tooltip(" Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown3 = 1.4f;
    [SerializeField, Tooltip(" Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay3 = 0f;



    // Logic Varibles
    private bool muscleTriggered = false;
    private string triggeredMuscles = "";
    private string triggeredMuscles2 = "";
    private readonly float delayTimer = 0.1f;
    private float currentTimer = 0;
    private float fireTimer;
    private float triggerTimer = 0f;
    private bool shouldProcess = false;
    private int triggerCount = 0;
    private string builtString = "";
    private string builtString2 = "";
    private string builtString3 = "";
    private string multiString = "";
    private bool doubletrigger = false;
    private bool triggered = false;
    private Rigidbody bullet;
    private bool fired;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private MeshCollider bulletCollider;
    private MeshRenderer bulletRenderer;
    private TrailRenderer trailRenderer;
    private bool showBulletTrail;

    private void Start()
    {
        bullet = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<MeshCollider>();
        bulletRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        bulletCollider.enabled = false;
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
    }
    public void SetSensationValues(int intensity,float speed,bool bulletTrail)
    {
        bulletSpeed = speed;
        if (intensity > 0)
        {
            intensityPercentage = intensity;
            intensityPercentage2 = intensity;
            intensityPercentage3 = intensity / 2;
            showBulletTrail = bulletTrail;
        }
    }
    private void Update()
    {
        if (shouldProcess)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= delayTimer && !triggered)
            {
                ProcessTriggeredZones();
                HandleHit();
                currentTimer = 0f;
                shouldProcess = false;
            }
        }
        if (triggered)
        {
            triggerTimer += Time.deltaTime;

            if (triggerTimer >= (doubletrigger ? duration2 : 0 + duration))
            {
                triggerTimer = 0f;
                shouldProcess = false;
                triggered = false;
                doubletrigger = false;
            }
        }
        if (fired)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= 10)
            {
                fireTimer = 0f;
                HandleHit();
            }
        }
    }
    public void FireBullet()
    {
        if (showBulletTrail) { trailRenderer.emitting = true; }
        transform.parent = null;
        bulletCollider.enabled = true;
        bullet.isKinematic = false;
        bullet.AddForce(bulletSpeed * transform.forward, ForceMode.VelocityChange);
        fired = true;
    }
    public void Reload()
    {
        bullet.isKinematic = true;
        bulletRenderer.enabled = true;
        transform.parent = originalParent;
        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
    }
    private void HandleHit()
    {
        if (showBulletTrail) { trailRenderer.emitting = false; }
        fired = false;
        bulletRenderer.enabled = false;
        bullet.isKinematic = true;
        bulletCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
            string currentMuscle = "";

            switch (other.name)
            {
                case pectoralL:
                    currentMuscle += pectoralLm;
                    break;
                case pectoralR:
                    currentMuscle += pectoralRm;
                    break;
                case dorsalL:
                    currentMuscle += dorsalLm;
                    break;
                case dorsalR:
                    currentMuscle += dorsalRm;
                    break;
                case armL:
                    currentMuscle += armLm;
                    break;
                case armR:
                    currentMuscle += armRm;
                    break;
                case lumbarL:
                    currentMuscle += lumbarLm;
                    break;
                case lumbarR:
                    currentMuscle += lumbarRm;
                    break;
                case abdominalL:
                    currentMuscle += abdominalLm;
                    break;
                case abdominalR:
                    currentMuscle += abdominalRm;
                    break;
                default:
                    return;
            }

            muscleTriggered = true;

            if (muscleTriggered)
            {
                if (triggerCount <= 1)
                {
                    triggerCount++;
                }
                shouldProcess = true;
            }
            if (triggerCount == 1)
            {
                triggeredMuscles = currentMuscle;
            }
            else if (triggerCount == 2)
            {
                triggeredMuscles2 = currentMuscle;
            }
            muscleTriggered = false;
    }

    private void ProcessTriggeredZones()
    {
        triggered = true;
        if (triggerCount == 1)
        {
            builtString = BuildSensationString(sensationName, frequency, duration, intensityPercentage, rampUp, rampDown, exitDelay, triggeredMuscles);
            builtString2 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggeredMuscles);
            multiString = BuildMultiSensationString(builtString, builtString2, builtString3);
            Debug.Log(multiString);
        }
        if (triggerCount == 2)
        {
            builtString = BuildSensationString(sensationName, frequency, duration, intensityPercentage, rampUp, rampDown, exitDelay, triggeredMuscles);
            builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggeredMuscles2);
            builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggeredMuscles + "," + triggeredMuscles2);
            multiString = BuildMultiSensationString(builtString, builtString2, builtString3);
            Debug.Log(multiString);
            doubletrigger = true;
            builtString3 = "";
        }
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
            return start + "\"priority\":" + sensationPriority + "," + sensationNameStart + sensationOne + separator + sensationNameStart + sensationTwo + separator + sensationNameStart + sensationThree + end;
        }
        if (sensationThree.Length <= 0)
        {
            return start + "\"priority\":" + sensationPriority + "," + sensationNameStart + sensationOne + separator + sensationNameStart + sensationTwo + end;
        }
        return "ERROR";
    }
}