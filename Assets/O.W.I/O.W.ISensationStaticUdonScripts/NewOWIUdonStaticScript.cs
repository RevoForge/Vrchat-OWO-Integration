using UdonSharp;
using UnityEngine;

public class NewOWIUdonStaticScript : UdonSharpBehaviour
{
[Header("Events")]
// Muscle Collider Names
private const string pectoralL = "owo_suit_Pectoral_L";
private const string pectoralR = "owo_suit_Pectoral_R";
private const string dorsalL = "owo_suit_Dorsal_L";
private const string dorsalR = "owo_suit_Dorsal_R";
private const string armL = "owo_suit_Arm_L";
private const string armR = "owo_suit_Arm_R";
private const string lumbarL = "owo_suit_Lumbar_L";
private const string lumbarR = "owo_suit_Lumbar_R";
private const string abdominalL = "owo_suit_Abdominal_L";
private const string abdominalR = "owo_suit_Abdominal_R";
// Muscle String Names
private readonly string pectoralLm = "\"pectoral_L\":";
private readonly string pectoralRm = "\"pectoral_R\":";
private readonly string dorsalLm = "\"dorsal_L\":";
private readonly string dorsalRm = "\"dorsal_R\":";
private readonly string armLm = "\"arm_L\":";
private readonly string armRm = "\"arm_R\":";
private readonly string lumbarLm = "\"lumbar_L\":";
private readonly string lumbarRm = "\"lumbar_R\":";
private readonly string abdominalLm = "\"abdominal_L\":";
private readonly string abdominalRm = "\"abdominal_R\":";
[SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")] private int sensationPriority = 1;
[SerializeField] private float colliderCaptureTimer = 0.1f;
// Muscle Intensity Ovverrides
private readonly int[] pectoralLmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] pectoralRmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] dorsalLmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] dorsalRmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] armLmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] armRmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] lumbarLmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] lumbarRmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] abdominalLmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly int[] abdominalRmIntValues = {100,100,100,100,100,100,100,100,100,100};
private readonly string start = "VRC_OWO_WorldIntegration:[";
private readonly string sensationNameStart = "\"sensation\": \"";
private readonly string separator = "}},{";
private readonly string end = "}}]";
private string[] triggerMusclesString = { "","","","","","","","","","",""};
// Logic Variables
private float currentTimer = 0f;
private float triggerTimer = 0f;
private bool shouldProcess = false;
private int triggerCount = 0;
private int triggeredMuscleInt = 0;
private string builtString1 = "";
private string builtString2 = "";
private string builtString3 = "";
private string multiString = "";
private float builtdurations = 0f;
private bool triggered = false;

[Header("Event 1")]
[SerializeField, Tooltip("Name for the Sensation event.")]
private string sensationName1 = "First";
[SerializeField, Tooltip("Frequency for the Sensation event.")]
[Range(1, 100)]
private int frequency1 = 100;
[SerializeField, Tooltip("Duration of the Sensation event.")]
[Range(0.1f,20)]
private float duration1 = 1f;
[SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
[Range(1, 100)]
private int intensityPercentage1 = 1;
[SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
[Range(0,2)]
private float rampUp1 =  0f ;
[SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
[Range(0,2)]
private float rampDown1 =  0f ;
[SerializeField, Tooltip("Exit delay for the Sensation event.")]
[Range(0,20)]
private float exitDelay1 =  0f ;
public MusclePlayOptions muscleOptions1;

[Header("Event 2")]
[SerializeField, Tooltip("Name for the Sensation event.")]
private string sensationName2 = "Second";
[SerializeField, Tooltip("Frequency for the Sensation event.")]
[Range(1, 100)]
private int frequency2 = 100;
[SerializeField, Tooltip("Duration of the Sensation event.")]
[Range(0.1f,20)]
private float duration2 = 1f;
[SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
[Range(1, 100)]
private int intensityPercentage2 = 1;
[SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
[Range(0,2)]
private float rampUp2 =  0f ;
[SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
[Range(0,2)]
private float rampDown2 =  0f ;
[SerializeField, Tooltip("Exit delay for the Sensation event.")]
[Range(0,20)]
private float exitDelay2 =  0f ;
public MusclePlayOptions muscleOptions2;

[Header("Event 3")]
[SerializeField, Tooltip("Name for the Sensation event.")]
private string sensationName3 = "Third";
[SerializeField, Tooltip("Frequency for the Sensation event.")]
[Range(1, 100)]
private int frequency3 = 100;
[SerializeField, Tooltip("Duration of the Sensation event.")]
[Range(0.1f,20)]
private float duration3 = 1f;
[SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
[Range(1, 100)]
private int intensityPercentage3 = 1;
[SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
[Range(0,2)]
private float rampUp3 =  0f ;
[SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
[Range(0,2)]
private float rampDown3 =  0f ;
[SerializeField, Tooltip("Exit delay for the Sensation event.")]
[Range(0,20)]
private float exitDelay3 =  0f ;
public MusclePlayOptions muscleOptions3;


private void Start()
{

    switch (muscleOptions1)
    {
        case MusclePlayOptions.PlayFirstMuscleTriggered:
            builtdurations += duration1;
            break;
        case MusclePlayOptions.PlayLastMuscleTriggered:
            builtdurations += duration1;
            break;
        case MusclePlayOptions.PlayMusclesTriggeredInOrder:
            builtdurations += duration1;
            break;
        case MusclePlayOptions.PlayAllMusclesTriggered:
            builtdurations += duration1;
            break;
    }

    switch (muscleOptions2)
    {
        case MusclePlayOptions.PlayFirstMuscleTriggered:
            builtdurations += duration2;
            break;
        case MusclePlayOptions.PlayLastMuscleTriggered:
            builtdurations += duration2;
            break;
        case MusclePlayOptions.PlayMusclesTriggeredInOrder:
            builtdurations += duration2;
            break;
        case MusclePlayOptions.PlayAllMusclesTriggered:
            builtdurations += duration2;
            break;
    }

    switch (muscleOptions3)
    {
        case MusclePlayOptions.PlayFirstMuscleTriggered:
            builtdurations += duration3;
            break;
        case MusclePlayOptions.PlayLastMuscleTriggered:
            builtdurations += duration3;
            break;
        case MusclePlayOptions.PlayMusclesTriggeredInOrder:
            builtdurations += duration3;
            break;
        case MusclePlayOptions.PlayAllMusclesTriggered:
            builtdurations += duration3;
            break;
    }
}

private void Update()
{
        if (shouldProcess)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= colliderCaptureTimer && !triggered)
            {
                ProcessTriggeredZones();
                currentTimer = 0f;
                shouldProcess = false;
            }
        }
        if (triggered)
        {
            triggerTimer += Time.deltaTime;
            if (triggerTimer >= builtdurations)
            {
                triggerTimer = 0f;
                shouldProcess = false;
                triggered = false;
                triggerCount = 0;
                triggerMusclesString = new string[] { "", "", "", "", "", "", "", "", "", "" };
                triggeredMuscleInt = 0;
            }
        }
}

private void OnTriggerEnter(Collider other)
{
if (triggerCount < 10)
        {
string currentMuscle = "";
    switch (other.name)
    {
        case pectoralL:
            currentMuscle += pectoralLm + pectoralLmIntValues[triggerCount];
            break;
        case pectoralR:
            currentMuscle += pectoralRm + pectoralRmIntValues[triggerCount];
            break;
        case dorsalL:
            currentMuscle += dorsalLm + dorsalLmIntValues[triggerCount];
            break;
        case dorsalR:
            currentMuscle += dorsalRm + dorsalRmIntValues[triggerCount];
            break;
        case armL:
            currentMuscle += armLm + armLmIntValues[triggerCount];
            break;
        case armR:
            currentMuscle += armRm + armRmIntValues[triggerCount];
            break;
        case lumbarL:
            currentMuscle += lumbarLm + lumbarLmIntValues[triggerCount];
            break;
        case lumbarR:
            currentMuscle += lumbarRm + lumbarRmIntValues[triggerCount];
            break;
        case abdominalL:
            currentMuscle += abdominalLm + abdominalLmIntValues[triggerCount];
            break;
        case abdominalR:
            currentMuscle += abdominalRm + abdominalRmIntValues[triggerCount];
            break;
        default:
            return;
    }
if (!string.IsNullOrEmpty(currentMuscle))
{
    triggerCount++;
    shouldProcess = true;
}
if (triggerCount == 1){triggerMusclesString[0] = currentMuscle;}
if (triggerCount == 2){triggerMusclesString[1] = currentMuscle;}
if (triggerCount == 3){triggerMusclesString[2] = currentMuscle;}
if (triggerCount == 4){triggerMusclesString[3] = currentMuscle;}
if (triggerCount == 5){triggerMusclesString[4] = currentMuscle;}
if (triggerCount == 6){triggerMusclesString[5] = currentMuscle;}
if (triggerCount == 7){triggerMusclesString[6] = currentMuscle;}
if (triggerCount == 8){triggerMusclesString[7] = currentMuscle;}
if (triggerCount == 9){triggerMusclesString[8] = currentMuscle;}
if (triggerCount == 10){triggerMusclesString[9] = currentMuscle;}
    }

}

private string ProcessTriggedMuscles()
{
string triggeredmuscles = "";
if (triggerMusclesString[0].Length > 0) {triggeredmuscles += triggerMusclesString[0] + ",";triggeredMuscleInt = 1;}
if (triggerMusclesString[1].Length > 0) {triggeredmuscles += triggerMusclesString[1] + ",";triggeredMuscleInt = 2;}
if (triggerMusclesString[2].Length > 0) {triggeredmuscles += triggerMusclesString[2] + ",";triggeredMuscleInt = 3;}
if (triggerMusclesString[3].Length > 0) {triggeredmuscles += triggerMusclesString[3] + ",";triggeredMuscleInt = 4;}
if (triggerMusclesString[4].Length > 0) {triggeredmuscles += triggerMusclesString[4] + ",";triggeredMuscleInt = 5;}
if (triggerMusclesString[5].Length > 0) {triggeredmuscles += triggerMusclesString[5] + ",";triggeredMuscleInt = 6;}
if (triggerMusclesString[6].Length > 0) {triggeredmuscles += triggerMusclesString[6] + ",";triggeredMuscleInt = 7;}
if (triggerMusclesString[7].Length > 0) {triggeredmuscles += triggerMusclesString[7] + ",";triggeredMuscleInt = 8;}
if (triggerMusclesString[8].Length > 0) {triggeredmuscles += triggerMusclesString[8] + ",";triggeredMuscleInt = 9;}
if (triggerMusclesString[9].Length > 0) {triggeredmuscles += triggerMusclesString[9];triggeredMuscleInt = 10;}
triggeredmuscles.TrimEnd(',');
return triggeredmuscles;
}

private void ProcessTriggeredZones()
{
triggered = true;
string allMuscles = ProcessTriggedMuscles();
        switch (muscleOptions1)
        {
case MusclePlayOptions.PlayFirstMuscleTriggered:
builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[0]); 
break; 
case MusclePlayOptions.PlayLastMuscleTriggered:
builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[triggeredMuscleInt]); 
break; 
case MusclePlayOptions.PlayAllMusclesTriggered:
builtString1 = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, allMuscles); 
break; 
case MusclePlayOptions.PlayMusclesTriggeredInOrder:
for (int i = 1; i <= triggeredMuscleInt; i++)
{
string playinorderString = BuildSensationString(sensationName1, frequency1, duration1, intensityPercentage1, rampUp1, rampDown1, exitDelay1, triggerMusclesString[i-1]);
builtString1 += playinorderString;
if (i < triggeredMuscleInt)
{
builtString1 += separator + sensationNameStart;
}
}
break; 
}
        switch (muscleOptions2)
        {
case MusclePlayOptions.PlayFirstMuscleTriggered:
builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[0]); 
break; 
case MusclePlayOptions.PlayLastMuscleTriggered:
builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[triggeredMuscleInt]); 
break; 
case MusclePlayOptions.PlayAllMusclesTriggered:
builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, allMuscles); 
break; 
case MusclePlayOptions.PlayMusclesTriggeredInOrder:
for (int i = 1; i <= triggeredMuscleInt; i++)
{
string playinorderString = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggerMusclesString[i-1]);
builtString2 += playinorderString;
if (i < triggeredMuscleInt)
{
builtString2 += separator + sensationNameStart;
}
}
break; 
}
        switch (muscleOptions3)
        {
case MusclePlayOptions.PlayFirstMuscleTriggered:
builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggerMusclesString[0]); 
break; 
case MusclePlayOptions.PlayLastMuscleTriggered:
builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggerMusclesString[triggeredMuscleInt]); 
break; 
case MusclePlayOptions.PlayAllMusclesTriggered:
builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, allMuscles); 
break; 
case MusclePlayOptions.PlayMusclesTriggeredInOrder:
for (int i = 1; i <= triggeredMuscleInt; i++)
{
string playinorderString = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggerMusclesString[i-1]);
builtString3 += playinorderString;
if (i < triggeredMuscleInt)
{
builtString3 += separator + sensationNameStart;
}
}
break; 
}
multiString = BuildMultiSensationString(builtString1, builtString2, builtString3);
Debug.Log(multiString);
builtString1 = ""; builtString2 = ""; builtString3 = "";
triggerMusclesString = new string[] { "", "", "", "", "", "", "", "", "", "" };
        triggeredMuscleInt = 0;

}

private string BuildSensationString(string sensation, int frequency, float durationVal, int intensityVal, float rampUp, float rampDown, float exitDelayVal, string muscles)
{
return sensation + "\","
+ "\"frequency\": " + frequency + ","
+ "\"duration\": " + durationVal + ","
+ "\"intensity\": " + intensityVal + ","
+ "\"rampup\":" + rampUp + ","
+ "\"rampdown\":" + rampDown + ","
+ "\"exitdelay\":" + exitDelayVal + ","
+ "\"Muscles\": {" + muscles.TrimEnd(',');

}

private string BuildMultiSensationString(string builtString1, string builtString2, string builtString3)
{
return start + "{\"priority\":" + sensationPriority + "," + sensationNameStart + builtString1 + separator + sensationNameStart + builtString2 + separator + sensationNameStart + builtString3 + end;

}
}
