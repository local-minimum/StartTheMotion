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
        EditorGUI.BeginChangeCheck();
        var transListProp = serializedObject.FindProperty("transitions");
        transListProp.isExpanded = EditorGUILayout.Foldout(transListProp.isExpanded, "Transitions");
        if (transListProp.isExpanded)
        {
            int nFilled = 0;
            for (int i = 0, l = transListProp.arraySize; i < l; i++)
            {
                var item = transListProp.GetArrayElementAtIndex(i);
                if (item.objectReferenceValue != null) {
                    EditorGUILayout.BeginHorizontal();
                    if (EditorGUILayout.PropertyField(item))
                    {
                    }
                    if (GUILayout.Button("- Remove"))
                    {
                        transListProp.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                    nFilled++;
                }
            }
            int movedIndex = 0;
            for (int i=0, l=transListProp.arraySize; i<l; i++)
            {
                var item = transListProp.GetArrayElementAtIndex(i);
                if (item.objectReferenceValue != null)
                {
                    Debug.Log(item.objectReferenceValue);
                    if (i != movedIndex)
                    {
                        transListProp.GetArrayElementAtIndex(movedIndex).objectReferenceValue = item.objectReferenceValue;
                    }
                    movedIndex++;
                }
               
            }
            transListProp.arraySize = movedIndex;
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
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
            var inst = Activator.CreateInstance(types[typeIndex]);
            Debug.Log(inst);
            //stAnim.AddTransition(inst as AbstractStopMotionTransition);
            //serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndHorizontal();

    }
}
