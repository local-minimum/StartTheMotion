using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierLine))]
public class BezierLineEditor : Editor {

    Transform handleTransform;
    Quaternion handleRotation;
    BezierLine line;
    private const int lineSteps = 15;

    private void OnSceneGUI()
    {
        line = target as BezierLine;

        handleTransform = line.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Handles.color = Color.grey;
        for (int i=0; i<line.points.Length; i++)
        {
            DrawHandle(i);
            if (i > 0)
            {
                Handles.DrawLine(line.points[i-1], line.points[i]);
            }
        }

        Handles.color = Color.white;
        Vector3 lineStart = line.GetPoint(0f);
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = line.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;
        }
    }

    void DrawHandle(int index)
    {
        Vector3 pointGlobal = handleTransform.TransformPoint(line.points[index]);
        EditorGUI.BeginChangeCheck();
        pointGlobal = Handles.DoPositionHandle(pointGlobal, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Moved Point");
            EditorUtility.SetDirty(line);
            line.points[index] = handleTransform.InverseTransformPoint(pointGlobal);

        }


    }
}
