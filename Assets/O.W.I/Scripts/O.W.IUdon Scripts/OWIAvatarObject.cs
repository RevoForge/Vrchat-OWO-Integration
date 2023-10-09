
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;



public class OWIAvatarObject : UdonSharpBehaviour
{
    private Vector3 desktopOffset = new Vector3( 0f,0.5f,0f);
    private readonly float defaultScale = 1.9f;
    private Vector3 originalTransformScale;
    private float playerScale;
    private VRCPlayerApi localPlayer;
    private Vector3 bonePosition;
    private Vector3 rotationSubtraction;
    private Quaternion boneRotation;
    [SerializeField]
    private HumanBodyBones targetBone;
    private bool isInVR = false;
    public bool isArm = false;
    private VRCPlayerApi.TrackingData trackingData;

    private void Start()
    {
        originalTransformScale = transform.localScale;
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
        if (localPlayer != null && (!isArm || !isInVR))
        {
            bonePosition = localPlayer.GetBonePosition(targetBone);
            boneRotation = localPlayer.GetBoneRotation(targetBone);
            transform.SetPositionAndRotation(bonePosition, boneRotation);
        }
        if (localPlayer != null && isArm && isInVR)
        {
            if (targetBone == HumanBodyBones.LeftUpperArm)
            {
                trackingData = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand);
            }
            if (targetBone == HumanBodyBones.RightUpperArm)
            {
                trackingData = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand);
            }
                rotationSubtraction = trackingData.position - (Vector3.Lerp(localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position, trackingData.position, 0.7f));
                boneRotation = Quaternion.FromToRotation(Vector3.down, rotationSubtraction);
                bonePosition = localPlayer.GetBonePosition(targetBone);
                transform.SetPositionAndRotation(bonePosition, boneRotation);
        }
        if (localPlayer != null && playerScale != localPlayer.GetAvatarEyeHeightAsMeters())
        {
            playerScale = localPlayer.GetAvatarEyeHeightAsMeters();
            CompareScalingDifference(defaultScale, playerScale);
        }

    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            localPlayer = player;
            isInVR = player.IsUserInVR();
            if (!isInVR && isArm)
            {
                gameObject.GetComponent<CapsuleCollider>().center = desktopOffset;
                transform.GetChild(0).localPosition = desktopOffset;
            }
            playerScale = (float)Math.Round(player.GetAvatarEyeHeightAsMeters(), 2);
            CompareScalingDifference(defaultScale, playerScale);
        }
    }


}
