using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurveChained)), CanEditMultipleObjects]
public class BezierCurveChainedEditor : BezierCurveEditor {


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

    private const float anchorFactor = 1.5f;
    private void DrawAnchorHandle()
    {
        BezierCurveChained chainedCurve = curve as BezierCurveChained;
        Vector3 pointGlobal = chainedCurve.GlobalAnchorPoint;
        float size = HandleUtility.GetHandleSize(pointGlobal) * anchorFactor;
        Handles.color = Color.magenta;
        if (Handles.Button(pointGlobal, handleRotation, size * handleSize, size * pickSize, Handles.CircleCap))
        {
            selectedIndex = -2;
            editingCurve = curve;
        }

        if (selectedIndex == -2 && curve == editingCurve)
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
