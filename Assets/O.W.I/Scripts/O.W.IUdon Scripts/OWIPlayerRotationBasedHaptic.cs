
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class OWIPlayerRotationBasedHaptic : UdonSharpBehaviour

{
    private Vector3 playerPosition;
    private Quaternion playerRotation;
    private bool inFrontOfPlayer = false;
    private bool behindPlayer = false;
    private bool leftOfPlayer = false;
    private bool rightOfPlayer = false;
    private float currentTimer = 1f;
    [Header("Objects To Turn Off")]
    [SerializeField,Tooltip("Objects To Turn Off when this becomes Active Incase you have Multiple Constants sensations at the same priority This Value Can be Null")]
    private GameObject[] otherConstantSensationObjects;

    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority;
    [SerializeField]
    [Range(0, 100)]
    private int sensationIntensity = 100;
    [SerializeField]
    [Range(0.1f, 20f)]
    private float windDuration = 0.4f;
    private VRCPlayerApi localPlayer;

    private void OnEnable()
    {
        localPlayer = Networking.LocalPlayer;
        if (otherConstantSensationObjects != null)
        {
            foreach (GameObject objects in otherConstantSensationObjects)
            {
                objects.SetActive(false);
            }
        }
    }
    private void Update()
    {
        playerPosition = localPlayer.GetBonePosition(HumanBodyBones.Chest);
        playerRotation = localPlayer.GetBoneRotation(HumanBodyBones.Chest);
        CheckPlayerOrientation(playerPosition, playerRotation);
    }
    private void LateUpdate()
    {
        currentTimer += Time.deltaTime;

        if (inFrontOfPlayer)
        {
            if (currentTimer >= windDuration - 0.01f)
            {
                Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Front Wind\",\"frequency\": 100,\"duration\": {windDuration},\"intensity\": {sensationIntensity},\"rampup\":0.2,\"rampdown\":0.2,\"exitdelay\":0,\"Muscles\": {{\"frontMuscles\": 100}}}}]");
                currentTimer = 0f;
            }
            inFrontOfPlayer = false;
        }
        if (leftOfPlayer)
        {
            if (currentTimer >= windDuration - 0.01f)
            {
                Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Left Wind\",\"frequency\": 100,\"duration\": {windDuration},\"intensity\": {sensationIntensity},\"rampup\":0.2,\"rampdown\":0.2,\"exitdelay\":0,\"Muscles\": {{\"pectoral_L\": 100,\"dorsal_L\": 100,\"arm_L\": 100,\"lumbar_L\": 100,\"abdominal_L\": 100}}}}]");

                currentTimer = 0f;
            }
            leftOfPlayer = false;
        }
        if (rightOfPlayer)
        {
            if (currentTimer >= windDuration - 0.01f)
            {
                Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Right Wind\",\"frequency\": 100,\"duration\": {windDuration},\"intensity\": {sensationIntensity},\"rampup\":0.2,\"rampdown\":0.2,\"exitdelay\":0,\"Muscles\": {{\"pectoral_R\": 100,\"dorsal_R\": 100,\"arm_R\": 100,\"lumbar_R\": 100,\"abdominal_R\": 100}}}}]");
                currentTimer = 0f;
            }
            rightOfPlayer = false;
        }
        if (behindPlayer)
        {
            if (currentTimer >= windDuration - 0.01f)
            {
                Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Back Wind\",\"frequency\": 100,\"duration\": {windDuration},\"intensity\": {sensationIntensity},\"rampup\":0.2,\"rampdown\":0.2,\"exitdelay\":0,\"Muscles\": {{\"backMuscles\": 100}}}}]");
                currentTimer = 0f;
            }
            behindPlayer = false;
        }
    }

    private void CheckPlayerOrientation(Vector3 playerPosition, Quaternion playerRotation)
    {
        Vector3 playerForward = playerRotation * Vector3.forward;
        Vector3 toObject = this.transform.position - playerPosition;

        float dotProduct = Vector3.Dot(playerForward, toObject.normalized);

        float threshold = 0.45f; // you can adjust this value based on your needs

        if (dotProduct > threshold)
        {
            inFrontOfPlayer = true;
        }
        else if (dotProduct < -threshold)
        {
            behindPlayer = true;
        }
        else
        {
            Vector3 crossProduct = Vector3.Cross(playerForward, toObject);
            if (crossProduct.y > 0)
            {
                rightOfPlayer = true;
            }
            else
            {
                leftOfPlayer = true;
            }
        }
    }
}

