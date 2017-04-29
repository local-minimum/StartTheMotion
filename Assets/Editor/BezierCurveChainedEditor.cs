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
            base.DrawHandle(index);
        }
    }

    private const float anchorFactor = 1.5f;
    private void DrawAnchorHandle()
    {
        Vector3 pointGlobal = handleTransform.TransformPoint((curve as BezierCurveChained).anchorPoint);
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
            Undo.RecordObject(curve, "Moved Point");
            EditorUtility.SetDirty(curve);
            //curve.points[index] = handleTransform.InverseTransformPoint(pointGlobal);

        }
    }
}
