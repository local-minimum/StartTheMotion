using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurveChained)), CanEditMultipleObjects]
public class BezierCurveChainedEditor : BezierCurveEditor {

    BezierCurveChained chainedCurve;

    public override void OnInspectorGUI()
    {
        chainedCurve = target as BezierCurveChained;

        if (chainedCurve.anchor == null)
        {
            EditorGUILayout.HelpBox("Chained curve will malfunction without an anchor", MessageType.Error);
        }
        base.OnInspectorGUI();
        

        if (GUILayout.Button("Make relative child"))
        {
            chainedCurve.CloneToChild();
        }

    }

    protected override void OnSceneGUI()
    {
        chainedCurve = target as BezierCurveChained;
        if (chainedCurve.anchor == null)
        {            
            return;
        }
        base.OnSceneGUI();
    }

    protected override void DrawHandle(int index)
    {
        if (index == 0)
        {
            DrawAnchorHandle();
        }
        else
        {
            base.DrawHandle(index - 1);
        }
    }

    private const float anchorFactor = 3f;
    private void DrawAnchorHandle()
    {
        Vector3 pointGlobal = chainedCurve.GlobalAnchorPoint;
        float size = HandleUtility.GetHandleSize(pointGlobal) * anchorFactor;
        Handles.color = Color.magenta;
        if (Handles.Button(pointGlobal, handleRotation, size * handleSize, size * pickSize, Handles.SphereHandleCap))
        {
            selectedIndex = -2;
            selectedBezierItem = this;
        }

        if (selectedIndex == -2 && this == selectedBezierItem)
        {
            pointGlobal = Handles.DoPositionHandle(pointGlobal, handleRotation);

            Undo.RecordObject(curve, "Moved Anchor Time");
            EditorUtility.SetDirty(chainedCurve);
            //Debug.Log(Vector3.SqrMagnitude(pointGlobal - chainedCurve.AnchorPoint));
            chainedCurve.anchorTime = chainedCurve.anchor.TimeClosestTo(pointGlobal);
            
            curve.CalculateLength();
        }
    }
}
