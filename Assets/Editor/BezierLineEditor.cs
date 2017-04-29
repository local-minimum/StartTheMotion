using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierLine))]
public class BezierLineEditor : Editor {

    Transform handleTransform;
    Quaternion handleRotation;
    BezierLine line;

    private void OnSceneGUI()
    {
        line = target as BezierLine;

        handleTransform = line.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        DrawHandles(line.pt0, "pt2");
        DrawHandles(line.pt1, "pt1");

        Handles.color = Color.white;
        Handles.DrawLine(line.pt0, line.pt1);
    }

    void DrawHandles(Vector3 point, string target)
    {
        Vector3 pointGlobal = handleTransform.TransformPoint(point);
        EditorGUI.BeginChangeCheck();
        Handles.DoPositionHandle(pointGlobal, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Moved Point");
            EditorUtility.SetDirty(line);
            line.pt1 = handleTransform.InverseTransformPoint(pointGlobal);
            /*
            typeof(BezierLine).GetProperty(target).SetValue(
                line,
                handleTransform.InverseTransformPoint(pointGlobal),
                null);
            */
            Debug.Log("Moved Point " + line.pt1);
            serializedObject.ApplyModifiedProperties();
        }


    }
}
