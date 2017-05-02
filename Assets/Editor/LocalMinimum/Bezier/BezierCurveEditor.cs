using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve)), CanEditMultipleObjects]
public class BezierCurveEditor : BezierEditors {

    protected Transform handleTransform;
    protected Quaternion handleRotation;
    protected BezierCurve curve;

    protected int selectedIndex = -1;

    private const int lineSteps = 15;

    public override void OnInspectorGUI()
    {
        curve = target as BezierCurve;
        base.OnInspectorGUI();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Make Linear"))
        {
            curve.MakeLinear();
        }
        if (GUILayout.Button("Default Shape"))
        {
            curve.SetDefaultShape();
        }
        EditorGUILayout.EndHorizontal();
    }

    protected virtual void OnSceneGUI()
    {
        curve = target as BezierCurve;

        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Handles.color = Color.grey;
        int l = curve.N;

        DrawComponentLines(l);
        DrawCapButtons(l);

        DrawCurve();
    }

    protected void DrawCapButtons(int l)
    {
        for (int i = 0; i < l; i++)
        {
            DrawHandle(i);
        }
    }

    protected void DrawCurve()
    {
        Handles.color = Color.white;
        Vector3 lineStart = handleTransform.TransformPoint(curve.GetPoint(0f));
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = handleTransform.TransformPoint(curve.GetPoint(i / (float)lineSteps));
            Handles.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;
        }
    }

    protected void DrawComponentLines(int l)
    {
        switch (l)
        {
            case 4:
                Handles.DrawLine(
                    curve.GetComponentPointGlobal(0),
                    curve.GetComponentPointGlobal(1));

                Handles.DrawLine(
                    curve.GetComponentPointGlobal(2),
                    curve.GetComponentPointGlobal(3));
                break;
            case 3:
                Handles.DrawLine(
                    curve.GetComponentPointGlobal(0),
                    curve.GetComponentPointGlobal(1));

                Handles.DrawLine(
                    curve.GetComponentPointGlobal(1),
                    curve.GetComponentPointGlobal(2));
                break;
        }

    }

    protected const float handleSize = 0.04f;
    protected const float pickSize = 0.06f;

    protected virtual void DrawHandle(int index)
    {

        Vector3 pointGlobal = handleTransform.TransformPoint(curve.points[index]);
        float size = HandleUtility.GetHandleSize(pointGlobal);
        Handles.color = Color.white;
        if (Handles.Button(pointGlobal, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))
        {
            selectedIndex = index;
            selectedBezierItem = this;
        }

        if (selectedIndex == index && this == selectedBezierItem)
        {
            pointGlobal = Handles.DoPositionHandle(pointGlobal, handleRotation);       
            Undo.RecordObject(curve, "Moved Point");

            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(pointGlobal);
            curve.CalculateLength();
        }

    }
}
