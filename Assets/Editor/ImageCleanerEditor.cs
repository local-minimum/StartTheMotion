using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageCleaner))]
public class ImageCleanerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Clean Up"))
        {
            (target as ImageCleaner).CleanUp();
        }
    }
}
