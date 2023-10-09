
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;



public class OWIAvatarObjectManualOverride : UdonSharpBehaviour
{
    public GameObject bodyObjectPecL;
    public GameObject bodyObjectPecR;
    public GameObject bodyObjectDorL;
    public GameObject bodyObjectDorR;
    public GameObject bodyObjectArmL;
    public GameObject bodyObjectArmR;
    public GameObject bodyObjectLumL;
    public GameObject bodyObjectLumR;
    public GameObject bodyObjectAbdL;
    public GameObject bodyObjectAbdR;
    public Slider sliderXAxis;
    public Slider sliderYAxis;
    public Slider sliderZAxis;

    private GameObject avatarObject;
    private bool objectActive = false;


    public void BodyObjectPecLToggle()
    {
        avatarObject = bodyObjectPecL;
        HandleToggle();
    }
    public void BodyObjectPecRToggle()
    {
        avatarObject = bodyObjectPecR;
        HandleToggle();
    }

    public void BodyObjectDorLToggle()
    {
        avatarObject = bodyObjectDorL;
        HandleToggle();
    }

    public void BodyObjectDorRToggle()
    {
        avatarObject = bodyObjectDorR;
        HandleToggle();
    }

    public void BodyObjectArmLToggle()
    {
        avatarObject = bodyObjectArmL;
        HandleToggle();
    }

    public void BodyObjectArmRToggle()
    {
        avatarObject = bodyObjectArmR;
        HandleToggle();
    }

    public void BodyObjectLumLToggle()
    {
        avatarObject = bodyObjectLumL;
        HandleToggle();
    }

    public void BodyObjectLumRToggle()
    {
        avatarObject = bodyObjectLumR;
        HandleToggle();
    }

    public void BodyObjectAbdLToggle()
    {
        avatarObject = bodyObjectAbdL;
        HandleToggle();
    }

    public void BodyObjectAbdRToggle()
    {
        avatarObject = bodyObjectAbdR;
        HandleToggle();
    }


    private void HandleToggle()
    {
        sliderXAxis.value = avatarObject.GetComponent<CapsuleCollider>().center.x;
        sliderYAxis.value = avatarObject.GetComponent<CapsuleCollider>().center.y;
        sliderZAxis.value = avatarObject.GetComponent<CapsuleCollider>().center.z;
        objectActive = !objectActive;
    }

    void Update()
    {
        if (objectActive)
        {
            avatarObject.GetComponent<CapsuleCollider>().center = new Vector3(sliderXAxis.value, sliderYAxis.value, sliderZAxis.value);
            avatarObject.transform.GetChild(0).localPosition = new Vector3(sliderXAxis.value, sliderYAxis.value, sliderZAxis.value);
        }

    }
}
