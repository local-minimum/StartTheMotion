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
        if (GUILayout.Button("Reset Shape"))
        {
            curve.SetDefaultShape();
        }

        if (GUILayout.Button("Inv Direction"))
        {
            curve.InvertDirection();
        }

        if (GUILayout.Button("Create child"))
        {
            curve.CloneMakeChild();
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
        
        DrawComponentLines();
        DrawCapButtons();

        DrawCurve();
    }
    private static Color[] capColors = new Color[] {
        Color.magenta,
        Color.white,
        Color.white,
        Color.green
    };

    protected void DrawCapButtons()
    {
        if (curve.anchorLeft)
        {
            DrawAnchorHandle(0);
        } else
        {
            DrawHandle(0);
        }

        DrawHandle(1);

        if (curve.bezierType == BezierType.Cubic)
        {
            DrawHandle(2);
        }

        if (curve.anchorRight)
        {
            DrawAnchorHandle(3);
        }
        else
        {
            DrawHandle(3);
        }
    }

    protected void DrawCurve()
    {
        Handles.color = Color.white;
        Vector3 lineStart = curve.GetGlobalPoint(0);
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = curve.GetGlobalPoint(i / (float)lineSteps);
            Handles.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;
        }
    }

    protected void DrawComponentLines()
    {
        switch (curve.bezierType)
        {
            case BezierType.Cubic:
                Handles.DrawLine(
                    curve.GetComponentPointGlobal(0),
                    curve.GetComponentPointGlobal(1));

                Handles.DrawLine(
                    curve.GetComponentPointGlobal(2),
                    curve.GetComponentPointGlobal(3));
                break;
            case BezierType.Quadratic:
                Handles.DrawLine(
                    curve.GetComponentPointGlobal(0),
                    curve.GetComponentPointGlobal(1));

                Handles.DrawLine(
                    curve.GetComponentPointGlobal(1),
                    curve.GetComponentPointGlobal(3));
                break;
        }

    }

    protected const float handleSize = 0.04f;
    protected const float pickSize = 0.06f;

    protected virtual void DrawHandle(int index)
    {

        Vector3 pointGlobal = handleTransform.TransformPoint(curve.points[index]);
        float size = HandleUtility.GetHandleSize(pointGlobal);
        Handles.color = capColors[index];
        if (Handles.Button(pointGlobal, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
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

    private const float anchorFactor = 3f;

    private void DrawAnchorHandle(int index)
    {
        Vector3 pointGlobal = curve.GetGlobalPoint(index);
        float size = HandleUtility.GetHandleSize(pointGlobal) * anchorFactor;
        Handles.color = capColors[index];
        if (Handles.Button(pointGlobal, handleRotation, size * handleSize, size * pickSize, Handles.SphereHandleCap))
        {
            selectedIndex = -2;
            selectedBezierItem = this;
        }

        if (selectedIndex == -2 && this == selectedBezierItem)
        {
            pointGlobal = Handles.DoPositionHandle(pointGlobal, handleRotation);

            Undo.RecordObject(curve, "Moved Anchor Time");
            EditorUtility.SetDirty(curve);
            //Debug.Log(Vector3.SqrMagnitude(pointGlobal - chainedCurve.AnchorPoint));
            if (index == 0)
            {
                curve.anchoredTimeLeft = curve.anchorLeft.TimeClosestTo(pointGlobal);
            } else if (index == 3)
            {
                curve.anchoredTimeRight = curve.anchorRight.TimeClosestTo(pointGlobal);
            }
            curve.CalculateLength();
        }
    }

}
