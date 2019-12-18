using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TilePropertyDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty cursor = serializedObject.FindProperty("cursor");
        SerializedProperty terrainType = serializedObject.FindProperty("terrainType");
        SerializedProperty randomGeneration = serializedObject.FindProperty("randomGeneration");
        SerializedProperty tileGraphics = serializedObject.FindProperty("tileGraphics");
        SerializedProperty obstacleLayer = serializedObject.FindProperty("obstacleLayer");
        SerializedProperty highlightedColor = serializedObject.FindProperty("highlightedColor");
        SerializedProperty isTraversable = serializedObject.FindProperty("isTraversable");

        Tile tile = target as Tile;

        EditorGUILayout.PropertyField(cursor);
        EditorGUILayout.PropertyField(terrainType);

        if (tile.terrainType == Tile.TerrainType.None)
        {
            EditorGUILayout.PropertyField(randomGeneration);
        }
        else
        {
            tile.randomGeneration = false;
        }

        EditorGUILayout.PropertyField(tileGraphics);
        EditorGUILayout.PropertyField(obstacleLayer);
        EditorGUILayout.PropertyField(highlightedColor);
        EditorGUILayout.PropertyField(isTraversable);

        serializedObject.ApplyModifiedProperties();
    }
}
