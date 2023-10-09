#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OWIUdonSharpDynamicScriptCreator))]
public class OWIUdonSharpDynamicCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This is for dynamically creating Udon O.W.I Script in the editor. It's experimental and may require manual adjustment.UdonScripts Created With This Will Need a Manually Created Udon Asset, To Avoid this Make sure you have a Folder /O.W.ISensationDynamicUdonScripts, In your Assets Folder Then Create an Udon Script and Put its FileName in the FileName Box to save over it Minimum Number of Sensations is Two", MessageType.Info);
        base.OnInspectorGUI();  // Draw the default inspector

        OWIUdonSharpDynamicScriptCreator script = (OWIUdonSharpDynamicScriptCreator)target;
        if (GUILayout.Button("Generate Script"))  // Add a button
        {
            script.GenerateScript();
        }
    }
}

[CustomEditor(typeof(OWIUdonSharpStaticScriptCreator))]
public class OWIUdonSharpStaticScriptCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This is for dynamically creating Udon O.W.I Script in the editor. It's experimental and may require manual adjustment.UdonScripts Created With This Will Need a Manually Created Udon Asset, To Avoid this Make sure you have a Folder /O.W.ISensationStaticUdonScripts, In your Assets Folder Then Create an Udon Script and Put its FileName in the FileName Box to save over it Minimum Number of Sensations is Two", MessageType.Info);
        base.OnInspectorGUI();  // Draw the default inspector

        OWIUdonSharpStaticScriptCreator script = (OWIUdonSharpStaticScriptCreator)target;
        if (GUILayout.Button("Generate Script"))  // Add a button
        {
            script.GenerateScript();
        }
    }
}
#endif
