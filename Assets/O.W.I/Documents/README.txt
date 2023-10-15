# Vrchat OWO World Integration
 Created by RevoForge & SonoVr   
 Documentation by BassBoostedDuck
 # OWI Client
 For Players   
 OWI Client Files avalible in Releases   
 Link to Main Github 
 https://github.com/RevoForge/Vrchat-OWO-Integration/tree/main#client-setup
# OWI Unity Package
 For World Creators   
  UnityPackage For VRC OWO WorldIntegrator also avalible in Releases   
 Link to Main Github
 https://github.com/RevoForge/Vrchat-OWO-Integration/tree/main#unity-basic-setup  
 Project designed with VRCHAT 3.4.0 SDK  
 **Some free assets from the Unity Store are needed for prefabs:**  
 Required For the Massage Chair Prefabs  
 https://assetstore.unity.com/packages/3d/props/furniture/sci-fi-chair-116719  
 
 Required for the PaintBall Gun Prefabs   
 https://assetstore.unity.com/packages/3d/props/weapons/weapon-master-scifi-weapon-1-lite-134423  
   
 Required for the Massage Effects Prefab   
 https://assetstore.unity.com/packages/vfx/particles/spells/particle-ribbon-42866  
 https://assetstore.unity.com/packages/2d/textures-materials/deep-space-skybox-pack-11056  
 https://assetstore.unity.com/packages/audio/music/floating-through-sky-ambient-meditation-relaxing-background-musi-225219  
   
 Required For the Heavy Object Prefab   
 https://assetstore.unity.com/packages/3d/props/sports-equipment-dumbbell-216505  
   
Additional Documentaion:
Advanced Sensation Information PDF (Also Avalible in the Release Package under Assets/O.W.I/Documents/)   
[VRC OWO System World integration - Advanced.pdf](https://github.com/RevoForge/Vrchat-OWO-Integration/files/12910141/VRC.OWO.System.World.integration.-.Advanced.pdf)


Sensation Information Referenced in the Advanced Document:


    // Muscle Collider Names
    private readonly string pectoralL = "owo_suit_Pectoral_L";
    private readonly string pectoralR = "owo_suit_Pectoral_R";
    private readonly string dorsalL = "owo_suit_Dorsal_L";
    private readonly string dorsalR = "owo_suit_Dorsal_R";
    private readonly string armL = "owo_suit_Arm_L";
    private readonly string armR = "owo_suit_Arm_R";
    private readonly string lumbarL = "owo_suit_Lumbar_L";
    private readonly string lumbarR = "owo_suit_Lumbar_R";
    private readonly string abdominalL = "owo_suit_Abdominal_L";
    private readonly string abdominalR = "owo_suit_Abdominal_R";
    // Muscle String Names
    private readonly string pectoralLm = "\"pectoral_L\": 100";
    private readonly string pectoralRm = "\"pectoral_R\": 100";
    private readonly string dorsalLm = "\"dorsal_L\": 100";
    private readonly string dorsalRm = "\"dorsal_R\": 100";
    private readonly string armLm = "\"arm_L\": 100";
    private readonly string armRm = "\"arm_R\": 100";
    private readonly string lumbarLm = "\"lumbar_L\": 100";
    private readonly string lumbarRm = "\"lumbar_R\": 100";
    private readonly string abdominalLm = "\"abdominal_L\": 100";
    private readonly string abdominalRm = "\"abdominal_R\": 100";
    private readonly string frontm = "\"frontMuscles\": 100";
    private readonly string backm = "\"backMuscles\": 100";
    private readonly string allm = "\"allMuscles\": 100";

    // Sensation String Parts
    private string start = "VRC_OWO_WorldIntegration:[{\"priority\": ";
    private readonly string end = "}}]";

    // Inspector MicroSensation Setting
    [Header("MicroSensation Settings")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority = 1;
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName = "Default Name";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency = 100;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration = 0.1f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage = 100;
    [SerializeField, Tooltip("Ramp up time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp = 0f;
    [SerializeField, Tooltip("Ramp down time. Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown = 0f;

    // Building Sensation String
            string builtString = start + sensationPriority + "," 
            + "\"sensation\": " + sensationName + ","
            + "\"frequency\": " + frequency + ","
            + "\"duration\": " + duration + ","
            + "\"intensity\": " + intensityPercentage + ","
            + "\"rampup\": " + rampUp + ","
            + "\"rampdown\": " + rampDown + ","
            + "\"exitdelay\": " + 0 + ","
            + "\"Muscles\": {" + triggeredMuscles
            + end;

        Debug.Log(builtString);



     // Sensation Settings
    [Header("First Zone Triggered Event")]
    [Header("Sensation Building Settings")]
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority = 1;
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName = "Sword Stab";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency = 60;
    [SerializeField, Tooltip("Duration of the Sensation event.")]
    [Range(0.1f, 20)]
    private float duration = 0.3f;
    [SerializeField, Tooltip("Intensity percentage for the Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage = 100;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown = 0f;
    [SerializeField, Tooltip("Exit delay for the  Sensation event.")]
    [Range(0, 20)]
    private float exitDelay = 0f;

    [Header("Secondary Zone Triggered Event")]
    [SerializeField, Tooltip("Name for the Sensation event.")]
    private string sensationName2 = "Sword Stab Through";
    [SerializeField, Tooltip("Frequency for the Sensation event.")]
    [Range(1, 100)]
    private int frequency2 = 50;
    [SerializeField, Tooltip("Duration of the  Sensation event.")]
    [Range(0.1f, 20)]
    private float duration2 = 0.3f;
    [SerializeField, Tooltip("Intensity percentage for the  Sensation event.")]
    [Range(1, 100)]
    private int intensityPercentage2 = 80;
    [SerializeField, Tooltip("Ramp up time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampUp2 = 0f;
    [SerializeField, Tooltip("Ramp down time Where 0.1 = 100ms Only 0.1 Increments affect the Vest.")]
    [Range(0, 2)]
    private float rampDown2 = 0f;
    [SerializeField, Tooltip("Exit delay for the Sensation event.")]
    [Range(0, 20)]
    private float exitDelay2 = 0f;

    // String Parts
    private readonly string start = "VRC_OWO_WorldIntegration:[{ \"priority\":";
    private readonly string sensationNameStart = "\"sensation\": \"";
    private readonly string separator = "}},{";
    private readonly string end = "}}]";
    
    private void ProcessTriggeredZones()
    {
        if (triggerCount == 1)
        {
            builtString = BuildSensationString(sensationName, frequency, duration, intensityPercentage, rampUp, rampDown, exitDelay, triggeredMuscles);
            builtString2 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggeredMuscles);
            multiString = BuildMultiSensationString(builtString, builtString2, builtString3);
            Debug.Log(multiString);
        }
        if (triggerCount == 2)
        {
            builtString = BuildSensationString(sensationName, frequency, duration, intensityPercentage, rampUp, rampDown, exitDelay, triggeredMuscles);
            builtString2 = BuildSensationString(sensationName2, frequency2, duration2, intensityPercentage2, rampUp2, rampDown2, exitDelay2, triggeredMuscles2);
            builtString3 = BuildSensationString(sensationName3, frequency3, duration3, intensityPercentage3, rampUp3, rampDown3, exitDelay3, triggeredMuscles + ","+triggeredMuscles2);
            multiString = BuildMultiSensationString(builtString, builtString2, builtString3);
            Debug.Log(multiString);
        }
        triggerCount = 0;
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
                + "\"Muscles\": {" + muscles;
    }
    
        private string BuildMultiSensationString(string sensationOne, string sensationTwo, string sensationThree)
    {
        if (triggerCount == 2)
        {
            return start + sensationPriority + "," + sensationNameStart + sensationOne + separator + sensationNameStart + sensationTwo + separator + sensationNameStart + sensationThree + end;
        }
        if (triggerCount == 1)
        {
            return start + sensationPriority + "," + sensationNameStart + sensationOne + separator + sensationNameStart + sensationTwo + end;
        }
        return "ERROR";
    }

    
