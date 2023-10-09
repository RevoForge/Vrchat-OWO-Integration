
using UdonSharp;
using UnityEngine;

public class ToggleStateButton : UdonSharpBehaviour
{
    [Tooltip("List of objects to toggle on and off")]
    public GameObject[] toggleObjects;
    public GameObject[] reverseToggleObjects;

    private bool toggleState = false;

    public override void Interact()
    {
        toggleState = !toggleState; // Toggle the state

        foreach (GameObject toggleObject in toggleObjects)
        {
            if (toggleObject == null)
                continue;  // Skip to the next iteration if toggleObject is null

            toggleObject.SetActive(toggleState); // Set active status based on toggleState
        }
        foreach (GameObject toggleObject in reverseToggleObjects)
        {
            if (toggleObject == null)
                continue;  // Skip to the next iteration if toggleObject is null

            toggleObject.SetActive(!toggleState); // Set active status based on toggleState
        }
    }
}
