using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class OWIObjectPickup : UdonSharpBehaviour
{
    private VRC_Pickup pickup;
    private float lastDebugTime = 0f;
    private bool pickupedR = false;
    private bool pickupedL = false;
    [Header("Event On Pickup")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority;
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency = 20;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration = 2.0f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage = 100;
    [SerializeField, Tooltip("Ramp up time on Pickup Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp = 0.8f;
    [SerializeField, Tooltip("Ramp down time on Pickup Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown = 0f;

    [Header("Holding Object Ramp settings")]
    [SerializeField, Tooltip("Ramp up time while Holding Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    private float secondaryrampUp = 0f;
    [SerializeField, Tooltip("Ramp down time while Holding Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    private float secondaryrampDown = 0f;

    private void Start()
    {
        pickup = GetComponent<VRC_Pickup>();
        if (pickup == null)
        {
            Debug.LogError("VRC_Pickup component not found!");
            return;
        }
    }

    private void Update()
    {
        if (pickup.IsHeld && (!pickupedL || !pickupedR) && pickup.currentPlayer == Networking.LocalPlayer)
        {
            if (pickup.currentHand == VRC_Pickup.PickupHand.Right && !pickupedR)
            {
                Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Weight\",\"frequency\": {frequency},\"duration\": {duration},\"intensity\": {intensityPercentage},\"rampup\":{rampUp},\"rampdown\":{rampDown},\"exitdelay\":0,\"Muscles\": {{\"arm_R\": 100,\"pectoral_R\": 50,\"dorsal_R\": 25}}}}]");
                pickupedR = true;
            }
            else if (pickup.currentHand == VRC_Pickup.PickupHand.Left && !pickupedL)
            {
                Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Weight\",\"frequency\": {frequency},\"duration\": {duration},\"intensity\": {intensityPercentage},\"rampup\":{rampUp},\"rampdown\":{rampDown},\"exitdelay\":0,\"Muscles\": {{\"arm_L\": 100,\"pectoral_L\": 50,\"dorsal_L\": 25}}}}]");
                pickupedL = true;
            }
        }
        if (pickupedR && Time.time - lastDebugTime >= (duration - 0.01f) && pickup.currentPlayer == Networking.LocalPlayer)
        {
            Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Weight\",\"frequency\": {frequency},\"duration\": {duration},\"intensity\": {intensityPercentage},\"rampup\":{secondaryrampUp},\"rampdown\":{secondaryrampDown},\"exitdelay\":0,\"Muscles\": {{\"arm_R\": 100,\"pectoral_R\": 50,\"dorsal_R\": 25}}}}]");
            lastDebugTime = Time.time;
        }
        if (pickupedL && Time.time - lastDebugTime >= (duration - 0.01f) && pickup.currentPlayer == Networking.LocalPlayer)
        {
            Debug.Log($"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Weight\",\"frequency\": {frequency},\"duration\": {duration},\"intensity\": {intensityPercentage},\"rampup\":{secondaryrampUp},\"rampdown\":{secondaryrampDown},\"exitdelay\":0,\"Muscles\": {{\"arm_R\": 100,\"pectoral_R\": 50,\"dorsal_R\": 25}}}}]");
            lastDebugTime = Time.time;
        }
        if (pickup.currentHand == VRC_Pickup.PickupHand.Left && pickupedR)
        {
            pickupedL = true;
            pickupedR = false;
        }
        if (pickup.currentHand == VRC_Pickup.PickupHand.Right && pickupedL)
        {
            pickupedR = true;
            pickupedL = false;
        }
        if (!pickup.IsHeld && pickup.currentPlayer == Networking.LocalPlayer && (pickupedL || pickupedR))
        {
            pickupedR = false;
            pickupedL = false;
            Debug.Log("VRC_OWO_WorldIntegration:[{\"sensation\":\"STOP\"}]");
        }
    }
}

