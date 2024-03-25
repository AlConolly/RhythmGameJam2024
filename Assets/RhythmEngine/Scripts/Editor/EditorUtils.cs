using UnityEditor;
using UnityEngine;

namespace RhythmEngine
{
    public static class EditorUtils
    {
        public static void Warning(string text, bool autoLayout = true)
        {
            if (autoLayout) EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", EditorStyles.WarningIcon, GUILayout.Width(16), GUILayout.Height(16));
            EditorGUILayout.LabelField(text, EditorStyles.Label);
            if (autoLayout) EditorGUILayout.EndHorizontal();
        }

        public static void Info(string text, bool autoLayout = true)
        {
            if (autoLayout) EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", EditorStyles.InfoIcon, GUILayout.Width(16), GUILayout.Height(16));
            EditorGUILayout.LabelField(text, EditorStyles.Label);
            if (autoLayout) EditorGUILayout.EndHorizontal();
        }
    }
}
