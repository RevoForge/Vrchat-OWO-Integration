using UdonSharp;
using UnityEngine;

public class MassageExperience : UdonSharpBehaviour
{
    [SerializeField]
    private ParticleSystem[] massages;
    private AudioSource massageAudioSource;

    [SerializeField]
    private Material massageSkybox;
    private Material originalSkybox;
    [SerializeField]
    private Light worldLight;

    private float skyboxRotation = 0f; // Current rotation of the skybox
    [SerializeField]
    private float rotationSpeed = 0.5f; // The speed of the rotation. You can adjust this value in the Inspector.

    void Start()
    {
        massageAudioSource = GetComponent<AudioSource>();
        originalSkybox = RenderSettings.skybox;
    }

    void Update()
    {
        // Check if the current skybox is the massageSkybox
        if (RenderSettings.skybox == massageSkybox)
        {
            skyboxRotation += Time.deltaTime * rotationSpeed;
            skyboxRotation %= 360f;
            massageSkybox.SetFloat("_Rotation", skyboxRotation);
        }
    }

    public void OnMassageChairUse()
    {
        foreach (ParticleSystem massage in massages)
        {
            massage.Play();
            massageAudioSource.Play();
        }

        // Set the skybox to the massage skybox
        RenderSettings.skybox = massageSkybox;
        worldLight.intensity = 0f;
    }

    public void OnMassageChairExit()
    {
        foreach (ParticleSystem massage in massages)
        {
            massage.Stop();
            massageAudioSource.Stop();
        }

        // Revert the skybox to the original
        RenderSettings.skybox = originalSkybox;
        worldLight.intensity = 1f;
    }
}
