using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(StopMotionAnimator))]
public class StopMotionAnimatorEditor : Editor {

    StopMotionAnimator stAnim;
    int typeIndex = 0;
    public override void OnInspectorGUI()
    {
        stAnim = target as StopMotionAnimator;

        ListTransitions();
        AddNewTransitions();
    }

    void ListTransitions()
    {
        var transListProp = serializedObject.FindProperty("transitions");
        transListProp.isExpanded = EditorGUILayout.Foldout(transListProp.isExpanded, "Transitions");
        if (transListProp.isExpanded)
        {

        } else
        {
            if (transListProp.arraySize == 0)
            {
                EditorGUILayout.HelpBox(
                    "No transition means nothing will animate",
                    MessageType.Warning);

            }
            else
            {
                EditorGUILayout.HelpBox(
                    string.Format("{0} Transitions {1} Sequences", transListProp.arraySize, serializedObject.FindProperty("sequences").arraySize),
                    MessageType.Info);
            }
        }
    }

    void AddNewTransitions()
    {
        var transitionInterface = typeof(AbstractStopMotionTransition);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => transitionInterface.IsAssignableFrom(p) && p != transitionInterface)
            .ToArray();

        EditorGUILayout.BeginHorizontal();
        typeIndex = EditorGUILayout.Popup(new GUIContent("New transition"), typeIndex, types.Select(e => new GUIContent(e.Name)).ToArray());
        if (GUILayout.Button("+ Add"))
        {
            stAnim.AddTransition(Activator.CreateInstance(types[typeIndex]) as AbstractStopMotionTransition);
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndHorizontal();

    }
}
