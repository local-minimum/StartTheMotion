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
        base.OnInspectorGUI();
        
        //ListTransitions();

    }

    void ListTransitions()
    {
        EditorGUI.BeginChangeCheck();
        var transListProp = serializedObject.FindProperty("transitions");
        if (transListProp == null)
        {
            return;
        }
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
                    if (GUILayout.Button("-", GUILayout.Width(26)))
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
    
}
