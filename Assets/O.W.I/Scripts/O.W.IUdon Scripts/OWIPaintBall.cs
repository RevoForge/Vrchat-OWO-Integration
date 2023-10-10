
using UdonSharp;
using UnityEngine;

public class OWIPaintBall : UdonSharpBehaviour
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
    // PaintBall Parts
    private Transform originalParent;
    private Transform fireLocation;
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;
    private Rigidbody ballBody;
    private Vector3 originalPosition;
    private new ParticleSystem particleSystem;
    private Vector3 originalScale;
    // Logic variables
    private bool fired = false;
    [HideInInspector]
    public float paintballSpeed = 10f;
    [HideInInspector]
    public int intensity = 50;
    [HideInInspector]
    public float lagcorrection = 0.25f;
    [HideInInspector]
    public int sensationPriority;
    private bool hitsomthing;
    private bool hittrigger;

    void Start()
    {

        particleSystem = GetComponent<ParticleSystem>();
        ballBody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
        fireLocation = originalParent.Find("FireLocation");
        sphereCollider.enabled = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (fired && !hitsomthing)
        {
            HandleShotLogic(collision.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (fired && !hitsomthing)
        {
            hittrigger = true;
            string primaryMuscle;
            string secondaryMuscles;

            HandleShotLogic(other.transform);

            switch (other.name)
            {
                case pectoralL:
                    primaryMuscle = "pectoral_L";
                    secondaryMuscles = "\"pectoral_R\": 50,\"abdominal_L\": 50,\"abdominal_R\": 25,\"arm_L\": 25";
                    break;
                case pectoralR:
                    primaryMuscle = "pectoral_R";
                    secondaryMuscles = "\"pectoral_L\": 50,\"abdominal_R\": 50,\"abdominal_L\": 25,\"arm_R\": 25";
                    break;
                case dorsalL:
                    primaryMuscle = "dorsal_L";
                    secondaryMuscles = "\"dorsal_R\": 50,\"lumbar_L\": 50,\"lumbar_R\": 25,\"arm_L\": 25";
                    break;
                case dorsalR:
                    primaryMuscle = "dorsal_R";
                    secondaryMuscles = "\"dorsal_L\": 50,\"abdominal_R\": 50,\"lumbar_L\": 25,\"arm_R\": 25";
                    break;
                case armL:
                    primaryMuscle = "arm_L";
                    secondaryMuscles = "\"pectoral_L\": 50,\"dorsal_L\": 50";
                    break;
                case armR:
                    primaryMuscle = "arm_R";
                    secondaryMuscles = "\"pectoral_R\": 50,\"dorsal_R\": 50";
                    break;
                case lumbarL:
                    primaryMuscle = "lumbar_L";
                    secondaryMuscles = "\"lumbar_R\": 50,\"dorsal_L\": 50,\"dorsal_R\": 25";
                    break;
                case lumbarR:
                    primaryMuscle = "lumbar_R";
                    secondaryMuscles = "\"lumbar_L\": 50,\"dorsal_R\": 50,\"dorsal_L\": 25";
                    break;
                case abdominalL:
                    primaryMuscle = "abdominal_L";
                    secondaryMuscles = "\"pectoral_L\": 50,\"abdominal_R\": 50,\"pectoral_R\": 25";
                    break;
                case abdominalR:
                    primaryMuscle = "abdominal_R";
                    secondaryMuscles = "\"pectoral_R\": 50,\"abdominal_L\": 50,\"pectoral_L\": 25";
                    break;
                default:
                    return;
            }

            // Only get here if one of the cases matched
            LogMuscleSensation(primaryMuscle, secondaryMuscles);
        }
    }

    private void LogMuscleSensation(string primaryMuscle, string secondaryMuscles)
    {
        Debug.Log($"VRC_OWO_WorldIntegration: [{{\"priority\": {sensationPriority}, \"sensation\": \"PaintBall\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"{primaryMuscle}\": 100}}}},{{ \"sensation\": \"PaintBall\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{ {secondaryMuscles} }}}}]");
    }

    private void HandleShotLogic(Transform other)
    {
        meshRenderer.enabled = false;
        sphereCollider.enabled = false;
        ballBody.isKinematic = true;
        if (hittrigger)
        {
            float backwardOffset = lagcorrection;  
            Vector3 adjustedPosition = transform.position - transform.forward * backwardOffset;
            transform.position = adjustedPosition;
        }
        hitsomthing = true;
        fired = false;
        particleSystem.Play();
        transform.parent = other;
        transform.localScale = originalScale;

    }

    public void FirePaintBall()
    {
        sphereCollider.enabled = true;

        meshRenderer.enabled = true;
        transform.position = fireLocation.position;
        transform.parent = null;
        ballBody.isKinematic = false;
        ballBody.AddForce(paintballSpeed * fireLocation.forward, ForceMode.VelocityChange);
        fired = true;
    }

    public void ReloadPaintBall()
    {
        particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        fired = false;
        ballBody.isKinematic = true;
        transform.parent = originalParent;
        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        hitsomthing = false;
        hittrigger = false;
    }
}
