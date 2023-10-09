
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class OWIMassageChair : UdonSharpBehaviour
{
    private bool turnedOn = false;
    private float currentTimer;
    [Tooltip("Time In Seconds Between Massage Preset Restart")]
    public float massageTimer = 0.5f;
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority=1;
    [SerializeField] private MassageExperience massageExperience;
    private bool[] massageStates = new bool[4];
    private int intensity = 100;
    private Slider slider = null;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        massageStates[0] = true;
    }

    public override void Interact()
    {
        VRCPlayerApi interactingPlayer = Networking.LocalPlayer;

        if (interactingPlayer != null && interactingPlayer.isLocal)
        {
            interactingPlayer.UseAttachedStation();
        }
    }
    public override void OnStationEntered(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            turnedOn = true;
            massageExperience.OnMassageChairUse();
        }
    }
    public override void OnStationExited(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            turnedOn = false;   
            massageExperience.OnMassageChairExit();
        }
    }

    private void Update()
    {
        if (turnedOn)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= massageTimer)
            {
                for (int i = 0; i < massageStates.Length; i++)
                {
                    if (massageStates[i])
                    {
                        Debug.Log($"{GetMessage(i)}");
                    }
                }
                currentTimer = 0f;
            }
        }
    }
    public void MassageType(int type)
    {
        for (int i = 0; i < massageStates.Length; i++)
        {
            massageStates[i] = false;
        }
        if (type >= 1 && type <= 5)
        {
            massageStates[type - 1] = true;
        }
    }
    public void SetIntensity()
    {
        intensity = (int)slider.value;
    }

    private string GetMessage(int index)
    {
        switch (index)
        {
            case 0:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100,\"dorsal_R\": 100 }}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_L\": 100,\"lumbar_R\": 100 }}}}]";
            case 1:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100,\"lumbar_L\": 100 }}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{ \"dorsal_R\": 100,\"lumbar_R\": 100 }}}}]";
            case 2:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 15,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100,\"dorsal_R\": 100 }}}},{{ \"sensation\": \"Massage\",\"frequency\": 15,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_L\": 100,\"lumbar_R\": 100 }}}}]";
            case 3:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100}}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"dorsal_R\": 100}}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_L\": 100}}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_R\": 100}}}}]";
            case 4:
                return $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\":  {intensity} ,\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{\"dorsal_L\": 100,\"dorsal_R\": 100 }}}},{{ \"sensation\": \"Massage\",\"frequency\": 5,\"duration\": 0.2,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0.1,\"exitdelay\":0,\"Muscles\": {{ \"lumbar_L\": 100,\"lumbar_R\": 100 }}}}]";
            default:
                return "ERROR";
        }
    }
}
