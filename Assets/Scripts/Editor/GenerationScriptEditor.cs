using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerationScript))]
public class GenerationScriptEditor : Editor {

    public override void OnInspectorGUI() {
        GenerationScript genScript = (GenerationScript)target;

        // Variables
        EditorGUILayout.LabelField("Terrain Settings", EditorStyles.boldLabel);
        genScript.levelWidth  = EditorGUILayout.IntField("Level Width",  genScript.levelWidth);
        genScript.levelHeight = EditorGUILayout.IntField("Level Height", genScript.levelHeight);
        genScript.startLength = EditorGUILayout.IntField("Start Length", genScript.startLength);
        genScript.endLength   = EditorGUILayout.IntField("End Length",   genScript.endLength);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Terrain Tilemap", EditorStyles.boldLabel);
        genScript.tileTop     = (GameObject)EditorGUILayout.ObjectField("Tile Top",    genScript.tileTop,    typeof(GameObject), false);
        genScript.tileBottom  = (GameObject)EditorGUILayout.ObjectField("Tile Bottom", genScript.tileBottom, typeof(GameObject), false);
        EditorGUILayout.Space();

        // Buttons
        if(GUILayout.Button("Regenerate")) {
            genScript.RegenerateTerrain();
        }
    }

}
