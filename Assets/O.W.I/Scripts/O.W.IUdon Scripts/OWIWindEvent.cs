using UdonSharp;
using UnityEngine;

public class OWIWindEvent : UdonSharpBehaviour
{

    private readonly string pectoralL = "owo_suit_Pectoral_L";
    private readonly string dorsalL = "owo_suit_Dorsal_L";
    private readonly string armL = "owo_suit_Arm_L";
    private readonly string armR = "owo_suit_Arm_R";
    private int sensationPriority = 4;
    private string main;
    private readonly string end = "}}]";
    private readonly string front = "\"frontMuscles\": 100";
    private readonly string back = "\"backMuscles\": 100";
    private readonly string armLm = "\"arm_L\": 100";
    private readonly string armRm = "\"arm_R\": 100";
    private bool muscleTriggered = false;
    private string triggeredMuscles = "";
    private readonly float delayTimer = 0.1f;
    private float currentTimer = 0f;
    private bool shouldProcess = false;

    private void Start()
    {
        main = $"VRC_OWO_WorldIntegration:[{{\"priority\": {sensationPriority},\"sensation\": \"Wind\",\"frequency\": 100,\"duration\": 2,\"intensity\": 25,\"rampup\":0.5,\"rampdown\":0.5,\"exitdelay\":0,\"Muscles\": {{";
    }
    private void Update()
    {
        if (shouldProcess)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= delayTimer)
            {
                ProcessTriggeredZones();
                currentTimer = 0f;
                shouldProcess = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == pectoralL && !triggeredMuscles.Contains(front))
        {
            triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + front;
            muscleTriggered = true;
        }
        if (other.name == dorsalL && !triggeredMuscles.Contains(back))
        {
            triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + back;
            muscleTriggered = true;
        }
        if (other.name == armL && !triggeredMuscles.Contains(armLm))
        {
            triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + armLm;
            muscleTriggered = true;
        }
        if (other.name == armR && !triggeredMuscles.Contains(armRm))
        {
            triggeredMuscles += (triggeredMuscles == "" ? "" : ", ") + armRm;
            muscleTriggered = true;
        }

        if (muscleTriggered)
        {
            shouldProcess = true;
        }
    }

    private void ProcessTriggeredZones()
    {
        string builtString = main + triggeredMuscles + end;
        Debug.Log(builtString);

        triggeredMuscles = "";
        muscleTriggered = false;
    }
}
