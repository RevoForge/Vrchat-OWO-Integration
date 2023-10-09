using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[AddComponentMenu("Udon Sharp/Utilities/Interact Toggle")]
[UdonBehaviourSyncMode(BehaviourSyncMode.Any)]
public class CustomButton : UdonSharpBehaviour
{
    public GameObject[] toggleObjects;
    public GameObject[] checkOffObjects;

    public bool isNetworked = false;

    [UdonSynced] private bool[] toggleStates;
    [UdonSynced] private bool[] checkOffStates;
    private bool localPlayer = false;

    private void Start()
    {
        InitializeStates(toggleObjects, ref toggleStates);
        InitializeStates(checkOffObjects, ref checkOffStates);
    }

    private void InitializeStates(GameObject[] objects, ref bool[] states)
    {
        if (objects != null)
        {
            states = new bool[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i])
                    states[i] = objects[i].activeSelf;
            }
        }
    }

    public override void Interact()
    {
        if (isNetworked)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "ToggleObjectsNetworked");
            localPlayer = true;
        }
        else
        {
            ToggleObjectsLocal();
        }
    }

    public void ToggleObjectsNetworked()
    {
        ToggleObjectsLocal();
    }

    private void ToggleObjectsLocal()
    {
        // Check if checkToggleObjects is not null and has at least one element
        bool hasCheckToggleOffObjects = checkOffObjects != null && checkOffObjects.Length > 0;

        foreach (GameObject toggleObject in toggleObjects)
        {
            if (toggleObject == null)
                continue;  
            toggleObject.SetActive(!toggleObject.activeSelf);
        }
        if (hasCheckToggleOffObjects && localPlayer)
        {
            foreach (GameObject toggleObject in checkOffObjects)
            {
                if (toggleObject != null && !toggleObject.activeSelf)
                {
                    toggleObject.SetActive(true);
                }
            }
        }

        // Update the synced states
        UpdateStates(toggleObjects, ref toggleStates);
        UpdateStates(checkOffObjects, ref checkOffStates);
        if (localPlayer) localPlayer = false;
        // Request a manual sync if needed
        if (isNetworked)
            RequestSerialization();
    }
    private void UpdateStates(GameObject[] objects, ref bool[] states)
    {
        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i])
                    states[i] = objects[i].activeSelf;
            }
        }
    }

    public override void OnDeserialization()
    {
        SetObjectStates(toggleObjects, toggleStates);
        SetObjectStates(checkOffObjects, checkOffStates);
    }

    private void SetObjectStates(GameObject[] objects, bool[] states)
    {
        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] && states != null && i < states.Length)
                    objects[i].SetActive(states[i]);
            }
        }
    }
}
