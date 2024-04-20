using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManagerScript))]
public class LevelManagerScriptEditor : Editor {

    public override void OnInspectorGUI() {
        LevelManagerScript lvlScript = (LevelManagerScript)target;

        // Variables
        EditorGUILayout.LabelField("Level Settings", EditorStyles.boldLabel);
        lvlScript.scrollSpeed = EditorGUILayout.IntField("Scroll Speed", lvlScript.scrollSpeed);
        EditorGUILayout.Space();

        // Buttons
        if(GUILayout.Button("Start Move")) {
            lvlScript.StartMove();
        }

        if(GUILayout.Button("Stop Move")) {
            lvlScript.StopMove();
        }

        if(GUILayout.Button("Restart")) {
            lvlScript.Restart();
        }
    }

}
