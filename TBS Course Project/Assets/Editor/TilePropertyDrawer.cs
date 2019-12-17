using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TilePropertyDrawer : Editor
{
    SerializedProperty cursor;
    SerializedProperty terrainType;
    SerializedProperty tileGraphics;

    private void OnEnable()
    {
        cursor = serializedObject.FindProperty("cursor");
        terrainType = serializedObject.FindProperty("terrainType");
        tileGraphics = serializedObject.FindProperty("tileGraphics");
    }

    public override void OnInspectorGUI()
    {
        Tile tile = target as Tile;

        EditorGUILayout.PropertyField(cursor);
        EditorGUILayout.PropertyField(terrainType);

        if (tile.terrainType == Tile.TerrainType.None)
        {
            tile.randomGeneration = EditorGUILayout.Toggle("Random Generation", tile.randomGeneration);
        }

        EditorGUILayout.PropertyField(tileGraphics);

        serializedObject.ApplyModifiedProperties();
    }
}
