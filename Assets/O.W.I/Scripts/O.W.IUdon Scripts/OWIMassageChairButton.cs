using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public enum MassageTypes
{
    Type1 = 1,
    Type2 = 2,
    Type3 = 3,
    Type4 = 4,
    Custom = 5
}

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class OWIMassageChairButton : UdonSharpBehaviour
{
    private OWIMassageChair chair;
    private OWIMassageChairVariable chairV;
    private OWIMassageChairUpgrade chairU;

    [SerializeField]
    private MassageTypes massageType = MassageTypes.Type1;

    private void Start()
    {
        chair = GetComponentInParent<OWIMassageChair>();
        chairV = GetComponentInParent<OWIMassageChairVariable>();
        chairU = GetComponentInParent<OWIMassageChairUpgrade>();
    }

    public override void Interact()
    {
        VRCPlayerApi interactingPlayer = Networking.LocalPlayer; // Get the local player

        if (interactingPlayer != null && interactingPlayer.isLocal && chair != null)
        {
            chair.MassageType((int)massageType); // Cast the enum to int
        }
        if (interactingPlayer != null && interactingPlayer.isLocal && chairV != null)
        {
            chairV.MassageType((int)massageType); // Cast the enum to int
        }
        if (interactingPlayer != null && interactingPlayer.isLocal && chairU != null)
        {
            chairU.MassageType((int)massageType); // Cast the enum to int
        }

    }
}
