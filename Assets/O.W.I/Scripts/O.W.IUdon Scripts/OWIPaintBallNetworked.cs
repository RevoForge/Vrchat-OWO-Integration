
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class OWIPaintBallNetworked : UdonSharpBehaviour
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
    private Vector3 originalScale;
    private Vector3 originalNetworkScale;
    private Transform particleNetworked;
    private Transform particle;
    // Logic variables
    private bool fired = false;
    [HideInInspector]
    public float paintballSpeed = 10f;
    [HideInInspector]
    public int intensity = 50;
    private bool hitsomthing;
    private bool hitsomethingNetworked = false;
    [HideInInspector]
    public float lagcorrection = 0.25f;
    [HideInInspector]
    public int sensationPriority;

    void Start()
    {

        ballBody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        particleNetworked = transform.GetChild(0);
        particle = transform.GetChild(1);
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
        originalNetworkScale = particleNetworked.localScale;
        fireLocation = originalParent.Find("FireLocation");
        sphereCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (fired && !hitsomthing && !hitsomethingNetworked)
        {
            hitsomthing = true;
            HandleLocalShotLogic(collision.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (fired && !hitsomethingNetworked)
        {
            ballBody.velocity = Vector3.zero;
            string primaryMuscle;
            string secondaryMuscles;

            HandleNetworkedLogic(other.transform);

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
                    secondaryMuscles = "\"pectoral_L\": 50,\"dorsal_L\": 50,\"abdominal_L\": 25,\"lumbar_L\": 25";
                    break;
                case armR:
                    primaryMuscle = "arm_R";
                    secondaryMuscles = "\"pectoral_R\": 50,\"dorsal_R\": 50,\"abdominal_R\": 25,\"lumbar_R\": 25";
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
        Debug.Log($"VRC_OWO_WorldIntegration: [{{ \"priority\": {sensationPriority},\"sensation\": \"PaintBall\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"{primaryMuscle}\": 100}}}},{{ \"sensation\": \"PaintBall\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ {secondaryMuscles} }}}}]");
    }

    private void DisablePaintBall()
    {
        ballBody.isKinematic = true;
        sphereCollider.enabled = false;
        meshRenderer.enabled = false;
    }

    private void EnableParticle(Transform particleTransform, Transform parent)
    {
        if (particleTransform != particle)
        {
            float backwardOffset = lagcorrection;  // Change this to your desired distance.
            Vector3 adjustedPosition = particleTransform.position - particleTransform.forward * backwardOffset;
            particleTransform.position = adjustedPosition;
        }
        particleTransform.gameObject.SetActive(true);
        particleTransform.parent = parent;
        particleTransform.localScale = (particleTransform == particle) ? originalScale : originalNetworkScale;
    }

    private void ResetParticle(Transform particleTransform)
    {
        particleTransform.gameObject.SetActive(false);
        particleTransform.parent = transform;
        particleTransform.localScale = originalScale;
        particleTransform.localPosition = originalPosition;
        particleTransform.localRotation = Quaternion.identity;
    }

    private void HandleLocalShotLogic(Transform other)
    {
        DisablePaintBall();
        fired = false;
        EnableParticle(particle, other);
        transform.parent = other;
        transform.localScale = originalScale;
    }

    private void HandleNetworkedLogic(Transform other)
    {
        DisablePaintBall();
        hitsomethingNetworked = true;
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TriggeredNetworked");
        EnableParticle(particleNetworked, other);
        Networking.SetOwner(Networking.LocalPlayer, particleNetworked.gameObject);
    }

    public void TriggeredNetworked()
    {
        if (!hitsomethingNetworked)
        {
            DisablePaintBall();
            hitsomethingNetworked = true;
            EnableParticle(particleNetworked, transform.parent);
            ResetParticle(particle);
        }
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
        fired = false;
        ballBody.isKinematic = true;
        transform.parent = originalParent;
        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        hitsomthing = false;
        if (hitsomethingNetworked)
        {
            ResetParticle(particleNetworked);
            hitsomethingNetworked = false;
        }
        else
        {
            ResetParticle(particle);
        }
    }

}
