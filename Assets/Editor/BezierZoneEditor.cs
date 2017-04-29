using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierZone)), CanEditMultipleObjects]
public class BezierZoneEditor : BezierEditors {

    BezierZone zone;
    protected Quaternion handleRotation;

    private const int lineSteps = 9;

    protected void OnSceneGUI()
    {

        zone = target as BezierZone;
        if (zone.curve == null)
        {
            return;
        }

        //Handles.Label(zone.curve.GetPoint(zone.left), name);

        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            zone.transform.rotation : Quaternion.identity;

        Handles.color = zone.lineColor;
        
        DrawHandle(0);
        DrawHandle(1);

        DrawCurve();
    }

    protected const float handleSize = 0.1f;
    protected const float pickSize = 0.12f;

    int selectedIndex = -1;

    protected virtual void DrawHandle(int index)
    {

        Vector3 pointGlobal = zone.curve.GetGlobalPoint(zone.times[index]);
        float size = HandleUtility.GetHandleSize(pointGlobal);
        if (Handles.Button(pointGlobal, handleRotation, size * handleSize, size * pickSize, Handles.SphereHandleCap))
        {            
            selectedIndex = index;
            selectedBezierItem = this;
        }

        if (selectedIndex == index && selectedBezierItem == this)
        {
            pointGlobal = Handles.DoPositionHandle(pointGlobal, handleRotation);
            Undo.RecordObject(zone, "Moved Anchor Time");

            EditorUtility.SetDirty(zone);
            zone.times[index] =zone.curve.TimeClosestTo(pointGlobal);
            
        }

    }

    protected void DrawCurve()
    {
        Handles.color = zone.lineColor;
        Vector3 lineStart;// = zone.curve.GetGlobalPoint(zone.left);
        float l = zone.right - zone.left;
        for (int i = 1; i < lineSteps; i++)
        {
            lineStart = zone.curve.GetGlobalPoint(zone.left + l * i / (float)lineSteps);
            float size = HandleUtility.GetHandleSize(lineStart);

            Handles.SphereHandleCap(0, lineStart, Quaternion.identity, size * handleSize * 0.5f, EventType.Repaint);
            //Handles.DrawLine(lineStart, lineEnd);
            //lineStart = lineEnd;
        }
    }
}
