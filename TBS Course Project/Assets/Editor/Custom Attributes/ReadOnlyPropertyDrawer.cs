using UnityEngine;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class ReadOnlyPropertyDrawer : PropertyAttribute { }

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
    {
        bool wasEnabled = GUI.enabled;
        GUI.enabled = false;
        UnityEditor.EditorGUI.PropertyField(position, property, true);
        GUI.enabled = wasEnabled;
    }
}
#endif
