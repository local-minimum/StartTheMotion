using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PositionWithAnimation))]
public class PositionRecorderEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PositionWithAnimation pwa = target as PositionWithAnimation;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Step Anim"))
        {
            pwa.StepCurrentAnimation();
        }
        if (GUILayout.Button("Record"))
        {
            pwa.RecordPosition();
        }
        EditorGUILayout.EndHorizontal();
    }
}
