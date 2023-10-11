#if UNITY_EDITOR
using System;
using System.Globalization;
using System.Text.RegularExpressions;
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
[CustomEditor(typeof(OWIGlobalSensation))]
public class OWIGlobalSensationsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("If Start Sensation On Load is not enabled to start the sensation you need to call the StartSensation() method and to stop it in either case you call the StopSensation() method", MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(TextAsset))]
public class TextAssetInspector : Editor
{
    private const string urlPattern = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
    public override void OnInspectorGUI()
    {
        TextAsset textAsset = target as TextAsset;
        if (textAsset == null) return;

        string[] lines = textAsset.text.Split('\n');

        foreach (string line in lines)
        {
            Match match = Regex.Match(line, urlPattern);
            if (match.Success)
            {
                GUI.enabled = true; // Forcefully enable GUI

                string url = match.Value;

                int lastSlashIndex = url.LastIndexOf('/');
                string displayText = lastSlashIndex >= 0 && lastSlashIndex < url.Length - 1 ?
                                      url.Substring(lastSlashIndex + 1) : url;
                displayText = displayText.Replace('-', ' ');
                displayText = displayText.TrimEnd();
                displayText = Regex.Replace(displayText, @"\d+$", "").Trim();
                displayText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(displayText);

                if (GUILayout.Button(displayText))
                {
                    Application.OpenURL(url);
                }
            }
            else
            {
                EditorGUILayout.LabelField(line);
            }
        }
    }
}
#endif
