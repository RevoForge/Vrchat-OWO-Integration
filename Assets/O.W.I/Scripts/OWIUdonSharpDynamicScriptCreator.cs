using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class OWIUdonSharpDynamicScriptCreator : MonoBehaviour
{
    // This is for dynamically creating Udon O.W.I Script in the editor. 
    // It's experimental and may require manual adjustment.
    // UdonScripts Created With This Will Need a Manually Created Udon Asset

    [System.Serializable]
    public class SensationEvent
    {
        [Tooltip("Name for the Sensation event.")]
        public string sensationName = "One";
        [Tooltip("Frequency for the Sensation event.")]
        [Range(1, 100)]
        public int frequency = 60;
        [Tooltip("Duration of the Sensation event.")]
        [Range(0.1f, 20)]
        public float duration = 0.1f;
        [Tooltip("Intensity percentage for the Sensation event.")]
        [Range(1, 100)]
        public int intensityPercentage = 100;
        [Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
        [Range(0, 2)]
        public float rampUp = 0f;
        [Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
        [Range(0, 2)]
        public float rampDown = 0f;
        [Tooltip("Exit delay for the Sensation event.")]
        [Range(0, 20)]
        public float exitDelay = 0f;
        public MuscleOverrides muscleOverrides;
    }

    [System.Serializable]
    public class MuscleOverrides
    {
        public int pectoralLmInt = 100;
        public int pectoralRmInt = 100;
        public int dorsalLmInt = 100;
        public int dorsalRmInt = 100;
        public int armLmInt = 100;
        public int armRmInt = 100;
        public int lumbarLmInt = 100;
        public int lumbarRmInt = 100;
        public int abdominalLmInt = 100;
        public int abdominalRmInt = 100;
    }
    [Header("Script FileName")]
    [SerializeField]
    private string fileName = "NewOWIUdonStaticScript";
    [SerializeField, Tooltip("Enable if to Stop the Last Triggered Sensation From Playing on All Muscles")] private bool ignoreGroupedMuscleSensation;

    [Header("Chained Sensations")]
    [SerializeField]
    private List<SensationEvent> sensations = new List<SensationEvent>();
    private readonly string destinationPath = "Assets/O.W.I/O.W.ISensationDynamicUdonScripts/{FileName}.cs";
    private readonly string directoryPath = "Assets/O.W.I/O.W.ISensationDynamicUdonScripts";
    private string GenerateStaticDefinitions()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("[Header(\"Events\")]");
        builder.AppendLine("// Muscle Collider Names");
        builder.AppendLine("private const string pectoralL = \"owo_suit_Pectoral_L\";");
        builder.AppendLine("private const string pectoralR = \"owo_suit_Pectoral_R\";");
        builder.AppendLine("private const string dorsalL = \"owo_suit_Dorsal_L\";");
        builder.AppendLine("private const string dorsalR = \"owo_suit_Dorsal_R\";");
        builder.AppendLine("private const string armL = \"owo_suit_Arm_L\";");
        builder.AppendLine("private const string armR = \"owo_suit_Arm_R\";");
        builder.AppendLine("private const string lumbarL = \"owo_suit_Lumbar_L\";");
        builder.AppendLine("private const string lumbarR = \"owo_suit_Lumbar_R\";");
        builder.AppendLine("private const string abdominalL = \"owo_suit_Abdominal_L\";");
        builder.AppendLine("private const string abdominalR = \"owo_suit_Abdominal_R\";");
        builder.AppendLine("// Muscle String Names");
        builder.AppendLine("private readonly string pectoralLm = \"\\\"pectoral_L\\\":\";");
        builder.AppendLine("private readonly string pectoralRm = \"\\\"pectoral_R\\\":\";");
        builder.AppendLine("private readonly string dorsalLm = \"\\\"dorsal_L\\\":\";");
        builder.AppendLine("private readonly string dorsalRm = \"\\\"dorsal_R\\\":\";");
        builder.AppendLine("private readonly string armLm = \"\\\"arm_L\\\":\";");
        builder.AppendLine("private readonly string armRm = \"\\\"arm_R\\\":\";");
        builder.AppendLine("private readonly string lumbarLm = \"\\\"lumbar_L\\\":\";");
        builder.AppendLine("private readonly string lumbarRm = \"\\\"lumbar_R\\\":\";");
        builder.AppendLine("private readonly string abdominalLm = \"\\\"abdominal_L\\\":\";");
        builder.AppendLine("private readonly string abdominalRm = \"\\\"abdominal_R\\\":\";");
        builder.AppendLine($"[SerializeField, Tooltip(\"Enable to Stop the Last Triggered Sensation From Playing on All Muscles\")] private bool ignoreGroupedMuscleSensation = {ignoreGroupedMuscleSensation.ToString().ToLower()};");
        builder.AppendLine("[SerializeField, Tooltip(\"Value Decides if it interrupts the previous sensation\")] private int sensationPriority = 1;");
        builder.AppendLine("[SerializeField] private float colliderCaptureTimer = 0.1f;");
        builder.AppendLine("// Muscle Intensity Ovverrides");
        string[] muscleNames = new string[]
        {
    "pectoralLmIntValues",
    "pectoralRmIntValues",
    "dorsalLmIntValues",
    "dorsalRmIntValues",
    "armLmIntValues",
    "armRmIntValues",
    "lumbarLmIntValues",
    "lumbarRmIntValues",
    "abdominalLmIntValues",
    "abdominalRmIntValues"
        };
        foreach (string muscleName in muscleNames)
        {
            builder.Append($"private readonly int[] {muscleName} = {{");

            for (int i = 0; i < sensations.Count; i++)
            {
                int value = 100;
                if (i < sensations.Count)
                {
                    switch (muscleName)
                    {
                        case "pectoralLmIntValues":
                            value = sensations[i].muscleOverrides.pectoralLmInt;
                            break;
                        case "pectoralRmIntValues":
                            value = sensations[i].muscleOverrides.pectoralRmInt;
                            break;
                        case "dorsalLmIntValues":
                            value = sensations[i].muscleOverrides.dorsalLmInt;
                            break;
                        case "dorsalRmIntValues":
                            value = sensations[i].muscleOverrides.dorsalRmInt;
                            break;
                        case "armLmIntValues":
                            value = sensations[i].muscleOverrides.armLmInt;
                            break;
                        case "armRmIntValues":
                            value = sensations[i].muscleOverrides.armRmInt;
                            break;
                        case "lumbarLmIntValues":
                            value = sensations[i].muscleOverrides.lumbarLmInt;
                            break;
                        case "lumbarRmIntValues":
                            value = sensations[i].muscleOverrides.lumbarRmInt;
                            break;
                        case "abdominalLmIntValues":
                            value = sensations[i].muscleOverrides.abdominalLmInt;
                            break;
                        case "abdominalRmIntValues":
                            value = sensations[i].muscleOverrides.abdominalRmInt;
                            break;
                    }
                }
                builder.Append($"{value}");
                if (i < sensations.Count - 1)
                {
                    builder.Append(",");
                }
            }
            builder.AppendLine("};");
        }
        builder.AppendLine("private readonly string start = \"VRC_OWO_WorldIntegration:[\";");
        builder.AppendLine("private readonly string sensationNameStart = \"\\\"sensation\\\": \\\"\";");
        builder.AppendLine("private readonly string separator = \"}},{\";");
        builder.AppendLine("private readonly string end = \"}}]\";");
        builder.AppendLine("private string[] triggerMusclesString = { \"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"};");
        builder.AppendLine("// Logic Variables");
        builder.AppendLine("private float currentTimer = 0f;");
        builder.AppendLine("private float triggerTimer = 0f;");
        builder.AppendLine("private bool shouldProcess = false;");
        builder.AppendLine("private int triggerCount = 0;");
        for (int i = 1; i <= sensations.Count; i++)
        {
            builder.AppendLine($"private string builtString{i} = \"\";");
        }
        builder.AppendLine("private string multiString = \"\";");
        builder.AppendLine("private float builtdurations = 0f;");
        builder.AppendLine("private bool triggered = false;");

        return builder.ToString();
    }
    private string GenerateSensationFields(int index)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"[Header(\"Event {index}\")]");
        builder.AppendLine($"[SerializeField, Tooltip(\"Name for the Sensation event.\")]");
        builder.AppendLine($"private string sensationName{index} = \"{sensations[index - 1].sensationName}\";");
        builder.AppendLine($"[SerializeField, Tooltip(\"Frequency for the Sensation event.\")]");
        builder.AppendLine($"[Range(1, 100)]");
        builder.AppendLine($"private int frequency{index} = {sensations[index - 1].frequency};");
        builder.AppendLine($"[SerializeField, Tooltip(\"Duration of the Sensation event.\")]");
        builder.AppendLine($"[Range(0.1f,20)]");
        builder.AppendLine($"private float duration{index} = {sensations[index - 1].duration}f;");
        builder.AppendLine($"[SerializeField, Tooltip(\"Intensity percentage for the Sensation event.\")]");
        builder.AppendLine($"[Range(1, 100)]");
        builder.AppendLine($"private int intensityPercentage{index} = {sensations[index - 1].intensityPercentage};");
        builder.AppendLine($"[SerializeField, Tooltip(\"Ramp up time. Only 0.1 Increments affect the Vest.\")]");
        builder.AppendLine($"[Range(0,2)]");
        builder.AppendLine($"private float rampUp{index} =  {sensations[index - 1].rampUp}f ;");
        builder.AppendLine($"[SerializeField, Tooltip(\"Ramp down time. Only 0.1 Increments affect the Vest.\")]");
        builder.AppendLine($"[Range(0,2)]");
        builder.AppendLine($"private float rampDown{index} =  {sensations[index - 1].rampDown}f ;");
        builder.AppendLine($"[SerializeField, Tooltip(\"Exit delay for the Sensation event.\")]");
        builder.AppendLine($"[Range(0,20)]");
        builder.AppendLine($"private float exitDelay{index} =  {sensations[index - 1].exitDelay}f ;");

        return builder.ToString();
    }
    private string GenerateSensationLogic(int index)
    {
        return $@"builtdurations += duration{index};";
    }
    private string GenerateUpdateLogic()
    {
        return $@"        if (shouldProcess)
        {{
            currentTimer += Time.deltaTime;

            if (currentTimer >= colliderCaptureTimer && !triggered)
            {{
                ProcessTriggeredZones();
                currentTimer = 0f;
                shouldProcess = false;
            }}
        }}
        if (triggered)
        {{
            triggerTimer += Time.deltaTime;
            if (triggerTimer >= builtdurations)
            {{
                triggerTimer = 0f;
                shouldProcess = false;
                triggered = false;
                triggerCount = 0;
                triggerMusclesString = new string[] {{ """", """", """", """", """", """", """", """", """", """" }};
            }}
        }}";
    }
    private string GenerateOnTriggerLogic()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"if (triggerCount < {sensations.Count})\r\n        {{");
        stringBuilder.AppendLine("string currentMuscle = \"\";");
        stringBuilder.AppendLine("    switch (other.name)");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        case pectoralL:");
        stringBuilder.AppendLine($"            currentMuscle += pectoralLm + pectoralLmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case pectoralR:");
        stringBuilder.AppendLine($"            currentMuscle += pectoralRm + pectoralRmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case dorsalL:");
        stringBuilder.AppendLine($"            currentMuscle += dorsalLm + dorsalLmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case dorsalR:");
        stringBuilder.AppendLine($"            currentMuscle += dorsalRm + dorsalRmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case armL:");
        stringBuilder.AppendLine($"            currentMuscle += armLm + armLmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case armR:");
        stringBuilder.AppendLine($"            currentMuscle += armRm + armRmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case lumbarL:");
        stringBuilder.AppendLine($"            currentMuscle += lumbarLm + lumbarLmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case lumbarR:");
        stringBuilder.AppendLine($"            currentMuscle += lumbarRm + lumbarRmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case abdominalL:");
        stringBuilder.AppendLine($"            currentMuscle += abdominalLm + abdominalLmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        case abdominalR:");
        stringBuilder.AppendLine($"            currentMuscle += abdominalRm + abdominalRmIntValues[triggerCount];");
        stringBuilder.AppendLine("            break;");
        stringBuilder.AppendLine("        default:");
        stringBuilder.AppendLine("            return;");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("if (!string.IsNullOrEmpty(currentMuscle))");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    triggerCount++;");
        stringBuilder.AppendLine("    shouldProcess = true;");
        stringBuilder.AppendLine("}");
        for (int i = 1; i <= sensations.Count; i++)
        {
            stringBuilder.AppendLine($"if (triggerCount == {i}){{triggerMusclesString[{i - 1}] = currentMuscle;}}");
        }
        stringBuilder.AppendLine("    }");
        // Finally, return the entire generated code as a string.
        return stringBuilder.ToString();
    }
    private string GenerateProcessLogic()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("triggered = true;");


        for (int maxTrigger = 1; maxTrigger <= sensations.Count; maxTrigger++)
        {
            stringBuilder.AppendLine($@"if (triggerCount == {maxTrigger})
    {{");

            for (int i = 1; i <= maxTrigger; i++)
            {
                string muscles;

                if (i == maxTrigger)
                {
                    muscles = string.Join("+\",\"+", Enumerable.Range(0, maxTrigger).Select(j => $"triggerMusclesString[{j}]"));
                }
                else
                {
                    muscles = $"triggerMusclesString[{i - 1}]";
                }
                if (i == maxTrigger)
                {
                    stringBuilder.AppendLine("if (!ignoreGroupedMuscleSensation) {");
                    stringBuilder.AppendLine($"builtString{i} = BuildSensationString(sensationName{i}, frequency{i}, duration{i}, intensityPercentage{i}, rampUp{i}, rampDown{i}, exitDelay{i}, {muscles}); }}");
                    stringBuilder.AppendLine("else {");
                    stringBuilder.AppendLine($"builtString{i} = BuildSensationString(sensationName{i}, frequency{i}, duration{i}, intensityPercentage{i}, rampUp{i}, rampDown{i}, exitDelay{i}, triggerMusclesString[{i - 1}]); }}");
                }
                else
                {
                stringBuilder.AppendLine($"builtString{i} = BuildSensationString(sensationName{i}, frequency{i}, duration{i}, intensityPercentage{i}, rampUp{i}, rampDown{i}, exitDelay{i}, {muscles}); ");

                }

            }

            stringBuilder.AppendLine("}");
        }

        stringBuilder.AppendLine($"multiString = BuildMultiSensationString({string.Join(", ", Enumerable.Range(1, sensations.Count).Select(i => $"builtString{i}"))} , triggerCount);");
        stringBuilder.AppendLine($"Debug.Log(multiString);");
        stringBuilder.AppendLine(string.Join(" ", Enumerable.Range(1, sensations.Count).Select(i => $"builtString{i} = \"\";")));
        stringBuilder.AppendLine($"triggerMusclesString = new string[] {{ \"\", \"\", \"\", \"\", \"\", \"\", \"\", \"\", \"\", \"\" }};");
        return stringBuilder.ToString();
    }
    private string GenerateBuildStringLogic()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("return sensation + \"\\\",\"\r\n+ \"\\\"frequency\\\": \" + frequency + \",\"\r\n+ \"\\\"duration\\\": \" + durationVal + \",\"\r\n+ \"\\\"intensity\\\": \" + intensityVal + \",\"\r\n+ \"\\\"rampup\\\":\" + rampUp + \",\"\r\n+ \"\\\"rampdown\\\":\" + rampDown + \",\"\r\n+ \"\\\"exitdelay\\\":\" + exitDelayVal + \",\"\r\n+ \"\\\"Muscles\\\": {\" + muscles.TrimEnd(',');");


        return stringBuilder.ToString();
    }
    private string GenerateMultiStringLogic()
    {
        StringBuilder codeBuilder = new StringBuilder();

        codeBuilder.AppendLine("if (numberOfStrings == 1)");
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine($"    return start + \"{{\\\"priority\\\":\" + sensationPriority + \",\" + sensationNameStart + builtString1 + end;");
        codeBuilder.AppendLine("}");

        for (int i = 2; i <= sensations.Count; i++)
        {
            codeBuilder.AppendLine($"if (numberOfStrings == {i})");
            codeBuilder.AppendLine("{");
            codeBuilder.Append($"    return start + \"{{\\\"priority\\\":\" + sensationPriority + \",\" + sensationNameStart + builtString1");

            for (int j = 2; j <= i; j++)
            {
                codeBuilder.Append(" + separator + sensationNameStart + builtString" + j);
            }

            codeBuilder.AppendLine(" + end;");
            codeBuilder.AppendLine("}");
        }

        codeBuilder.AppendLine("return \"Number of Trigger Counts ERROR\";");

        return codeBuilder.ToString();
    }

    public void GenerateScript()
    {
        StringBuilder scriptContent = new StringBuilder();

        scriptContent.AppendLine("using UdonSharp;");
        scriptContent.AppendLine("using UnityEngine;\r\n");
        scriptContent.AppendLine($"public class {fileName} : UdonSharpBehaviour");
        scriptContent.AppendLine("{");

        scriptContent.AppendLine(GenerateStaticDefinitions());

        for (int i = 1; i <= sensations.Count; i++)
        {
            scriptContent.AppendLine(GenerateSensationFields(i));
        }

        scriptContent.AppendLine("\r\nprivate void Start()");
        scriptContent.AppendLine("{");
        for (int i = 1; i <= sensations.Count; i++)
        {
            scriptContent.AppendLine(GenerateSensationLogic(i));
        }
        scriptContent.AppendLine("}");

        scriptContent.AppendLine("\r\nprivate void Update()");
        scriptContent.AppendLine("{");
        scriptContent.AppendLine(GenerateUpdateLogic());
        scriptContent.AppendLine("}");

        scriptContent.AppendLine("\r\nprivate void OnTriggerEnter(Collider other)");
        scriptContent.AppendLine("{");
        scriptContent.AppendLine(GenerateOnTriggerLogic());
        scriptContent.AppendLine("}");

        scriptContent.AppendLine("\r\nprivate void ProcessTriggeredZones()");
        scriptContent.AppendLine("{");
        scriptContent.AppendLine(GenerateProcessLogic());
        scriptContent.AppendLine("}");

        scriptContent.AppendLine("\r\nprivate string BuildSensationString(string sensation, int frequency, float durationVal, int intensityVal, float rampUp, float rampDown, float exitDelayVal, string muscles)");
        scriptContent.AppendLine("{");
        scriptContent.AppendLine(GenerateBuildStringLogic());
        scriptContent.AppendLine("}");

        scriptContent.AppendLine($"\r\nprivate string BuildMultiSensationString({"string " + string.Join(", string ", Enumerable.Range(1, sensations.Count).Select(i => $"builtString{i}"))}, int numberOfStrings)");
        scriptContent.AppendLine("{");
        scriptContent.AppendLine(GenerateMultiStringLogic());
        scriptContent.AppendLine("}");




        scriptContent.AppendLine("}");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string path = destinationPath.Replace("{FileName}", fileName);
        File.WriteAllText(path, scriptContent.ToString());

#if UNITY_EDITOR
        AssetDatabase.Refresh();
        // Notify in console
        Debug.Log("Script Generated at: " + path);
#endif
    }
}
