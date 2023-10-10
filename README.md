# Vrchat-OWO-Integration
 World Creator Files For VRC_OWO_WorldIntegrator Program Version PR1   
 
 Created by RevoForge & SonoVr   
 Documentation by BassBoostedDuck
 
 Project designed with VRCHAT 3.4.0 SDK  
 
 Free Assets from the Store are needed for prefabs  
 https://assetstore.unity.com/packages/3d/props/furniture/sci-fi-chair-116719  
 Required For the Massage Chair Prefabs  
 https://assetstore.unity.com/packages/3d/props/weapons/weapon-master-scifi-weapon-1-lite-134423  
 Required for the PaintBall Gun Prefabs  
 https://assetstore.unity.com/packages/vfx/particles/spells/particle-ribbon-42866  
 https://assetstore.unity.com/packages/2d/textures-materials/deep-space-skybox-pack-11056  
 https://assetstore.unity.com/packages/audio/music/floating-through-sky-ambient-meditation-relaxing-background-musi-225219  
 Required for the Massage Effects Prefab  
 https://assetstore.unity.com/packages/3d/props/sports-equipment-dumbbell-216505  
 Required For the Heavy Object Prefab  

Basic Setup in PDF (Also Avalible in the Release Package under Assets/O.W.I/Documents/)   
[VRC OWO System World integration - Basic.pdf](https://github.com/RevoForge/Vrchat-OWO-Integration/files/12851576/VRC.OWO.System.World.integration.-.Basic.pdf)

Advanced Sensation Information PDF (Also Avalible in the Release Package under Assets/O.W.I/Documents/)   
[VRC OWO haptics World integration - Advanced.pdf](https://github.com/RevoForge/Vrchat-OWO-Integration/files/12851581/VRC.OWO.haptics.World.integration.-.Advanced.pdf)

Basic Setup
![VRC OWO System World integration - Basic(0)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/fecf1405-5bae-4122-bc7e-50104a07fbba)
![VRC OWO System World integration - Basic(1)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/538d49ad-6e1f-44a6-b978-84f72aa4cb41)
![VRC OWO System World integration - Basic(2)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/ea507622-6e9a-4ac5-be03-6e39dd3d7fad)
![VRC OWO System World integration - Basic(3)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/3c1e9b7c-e7fc-41f5-b4a8-f22c45cc8304)
![VRC OWO System World integration - Basic(4)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/b13e17b4-3e58-4c6d-8f95-f25fc691517c)
![VRC OWO System World integration - Basic(5)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/3d5e2b31-df75-4b9d-848b-abe768a1d4b4)
![VRC OWO System World integration - Basic(6)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/6fa9addb-e1d1-43c5-9e56-6b85b7198bb1)
![VRC OWO System World integration - Basic(7)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/242fcf57-39a5-421c-8774-dd5e2eb2ffba)
![VRC OWO System World integration - Basic(8)](https://github.com/RevoForge/Vrchat-OWO-Integration/assets/144636833/a1df1dc2-8c53-494f-ba23-28135e7d48e5)

Advanced Sensation Information;

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
