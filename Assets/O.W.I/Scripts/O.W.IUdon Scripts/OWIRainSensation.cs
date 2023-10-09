
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class OWIRainSensation : UdonSharpBehaviour
{
    // public GameObject playerRotationDebugObject; // Debug Object
    private float currentTimer = 0f;
    private readonly float standingThreshold = 1; // Used to adjust orientation detection
    private readonly float threshold = 0.45f; // Used to adjust orientation detection
    [Header("Objects To Turn Off")]
    [Tooltip("Objects To Turn Off when this becomes Active Incase you have Multiple Constants sensations at the same priority This Value Can be Null")]
    public GameObject[] otherConstantSensationObjects;
    [Header("Rain Settings")]
    [SerializeField, Tooltip("Value Decides if this interrupts a previous Sensation")]
    private int sensationPriority = 1;
    [SerializeField, Tooltip("Value Sets the Intensity of the Rain")]
    [Range(1, 100)]
    private int sensationIntensity = 70;
    [SerializeField, Tooltip("Value Sets the Length of the RainDrop feeling a new RainDrop wont be sent until the previous is done")]
    [Range(0.1f, 20)]
    private float sensationDuration = 0.1f;
    private Quaternion playerRotation;
    private bool facingUpwards = false;
    private bool facingDownwards = false;
    private bool onLeftSide = false;
    private bool onRightSide = false;
    private bool ranMuscleBuilder = false;
    private VRCPlayerApi localPlayer;
    private readonly string[] musclesArrayFront = { "\"pectoral_L\": 100", "\"pectoral_R\": 100", "\"arm_L\": 100", "\"arm_R\": 100", "\"abdominal_L\": 100", "\"abdominal_R\": 100" };
    private readonly string[] musclesArrayBack = { "\"dorsal_L\": 100", "\"dorsal_R\": 100", "\"lumbar_L\": 100", "\"lumbar_R\": 100" };
    private readonly string[] musclesArrayRight = { "\"pectoral_R\": 100", "\"dorsal_R\": 100", "\"arm_R\": 100", "\"lumbar_R\": 100", "\"abdominal_R\": 100" };
    private readonly string[] musclesArrayLeft = { "\"pectoral_L\": 100", "\"dorsal_L\": 100", "\"arm_L\": 100", "\"lumbar_L\": 100", "\"abdominal_L\": 100" };
    private readonly string[] musclesArrayFull = { "\"pectoral_L\": 100", "\"pectoral_R\": 100", "\"dorsal_L\": 100", "\"dorsal_R\": 100", "\"arm_L\": 100", "\"arm_R\": 100", "\"lumbar_L\": 100", "\"lumbar_R\": 100", "\"abdominal_L\": 100", "\"abdominal_R\": 100" };
    private string builtMuscles = "";
    private bool ranonce = false;

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
        // playerRotation = playerRotationDebugObject.transform.rotation; // for debugging laying logic

        playerRotation = localPlayer.GetBoneRotation(HumanBodyBones.Chest); // for live use
        if (!ranMuscleBuilder)CheckPlayerLayingOrientation(playerRotation);
        if (facingUpwards && !ranMuscleBuilder)
        {
            ranMuscleBuilder = true;
            builtMuscles = MuscleBuilder(musclesArrayFront);
            // Debug.Log("FrontArray"); //Used to Help adjust orientation detection
        }
        if (facingDownwards && !ranMuscleBuilder)
        {
            ranMuscleBuilder = true;
            builtMuscles = MuscleBuilder(musclesArrayBack);
            // Debug.Log("BackArray"); //Used to Help adjust orientation detection
        }
        if (onLeftSide && !ranMuscleBuilder)
        {
            ranMuscleBuilder = true;
            builtMuscles = MuscleBuilder(musclesArrayRight);
            // Debug.Log("RightArray"); //Used to Help adjust orientation detection
        }
        if (onRightSide && !ranMuscleBuilder)
        {
            ranMuscleBuilder = true;
            builtMuscles = MuscleBuilder(musclesArrayLeft);
            // Debug.Log("LeftArray"); //Used to Help adjust orientation detection
        }
        if (!onLeftSide && !onRightSide && !facingUpwards && !facingDownwards && !ranMuscleBuilder)
        {
            ranMuscleBuilder = true;
            builtMuscles = MuscleBuilder(musclesArrayFull);
            // Debug.Log("FullArray"); //Used to adjust orientation detection
        }

    }
    private string MuscleBuilder(string[] muscleSide)
    {
        int randomIndex = UnityEngine.Random.Range(0, muscleSide.Length);
        return muscleSide[randomIndex];
    }
    private void LateUpdate()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= sensationDuration + 0.05)
        {
            Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \" Raindrop\",\"frequency\": 50,\"duration\": {sensationDuration},\"intensity\": {sensationIntensity},\"rampup\":0,\"rampdown\":{sensationDuration},\"exitdelay\":0,\"Muscles\": {{{builtMuscles}}}}}]");
            currentTimer = 0f;
            facingUpwards = false;
            facingDownwards = false;
            onLeftSide = false;
            onRightSide = false;
            ranonce = false;
            builtMuscles = "";
        }
        if (currentTimer >= sensationDuration && !ranonce)
        {
            ranonce = true;
            ranMuscleBuilder = false;
        }
    }

    private void CheckPlayerLayingOrientation(Quaternion playerRotation)
    {
        Vector3 playerForward = playerRotation * Vector3.forward;
        Vector3 playerRight = playerRotation * Vector3.right;

        float dotProductForward = Vector3.Dot(playerForward, Vector3.up);
        float dotProductRight = Vector3.Dot(playerRight, Vector3.up);



        // Reset the flags
        facingUpwards = false;
        facingDownwards = false;
        onLeftSide = false;
        onRightSide = false;

        if (Math.Abs(dotProductForward) > standingThreshold)
        {
            return; // Player is standing.
        }
        else if (dotProductForward > threshold)
        {
            facingUpwards = true; // Laying on Back
        }
        else if (dotProductForward < -threshold)
        {
            facingDownwards = true; // Laying on Stomach
        }
        else if (dotProductRight > threshold)
        {
            onLeftSide = true; // Laying on left side
        }
        else if (dotProductRight < -threshold)
        {
            onRightSide = true; // Laying on right side
        }
    }
}
