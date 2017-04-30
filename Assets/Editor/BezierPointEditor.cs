using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierPoint)), CanEditMultipleObjects]
public class BezierPointEditor : BezierEditors {

    BezierPoint point;
    Transform handleTransform;
    Quaternion handleRotation;

    protected const float handleSize = 0.1f;
    protected const float pickSize = 0.12f;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset offset"))
        {
            point.anchorOffset = Vector3.zero;
            point.Snap();
        }
    }

    private void OnSceneGUI()
    {
        point = target as BezierPoint;
        handleTransform = point.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        DrawHandle();
    }

    protected virtual void DrawHandle()
    {

        Vector3 anchorGlobal = point.CurvePoint;

        float size = HandleUtility.GetHandleSize(anchorGlobal);
        Handles.color = Color.white;
        if (Handles.Button(anchorGlobal, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
        {
            selectedBezierItem = this;
        }

        if (this == selectedBezierItem)
        {
            
            Vector3 newPtGlobal = Handles.DoPositionHandle(anchorGlobal, handleRotation);

            Debug.Log("Moved " + handleTransform.InverseTransformDirection(anchorGlobal - newPtGlobal));
            Undo.RecordObject(point, "Moved Anchor Point");

            EditorUtility.SetDirty(point);
            point.anchorOffset += handleTransform.InverseTransformDirection(anchorGlobal-newPtGlobal) * 0.2f;
            point.Snap();
        }

    }

}
