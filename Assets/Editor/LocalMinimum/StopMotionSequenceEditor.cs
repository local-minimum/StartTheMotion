using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StopMotionSequencer))]
public class StopMotionSequenceEditor : Editor {

    StopMotionSequencer smSeq;

    public override void OnInspectorGUI()
    {
        smSeq = target as StopMotionSequencer;
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play"))
        {
            smSeq.Play();
        }
        if (GUILayout.Button("Stop"))
        {
            smSeq.Stop();
        }
        EditorGUILayout.EndHorizontal();
    }
}
