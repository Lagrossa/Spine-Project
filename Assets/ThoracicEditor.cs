using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ThoracicHandler)), CanEditMultipleObjects]
public class ThoracicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ThoracicHandler handler = (ThoracicHandler)target;
        if (GUILayout.Button("Rewrite Flexion")) { handler.WriteFlexion(Data.getInstance()); }
        if (GUILayout.Button("Flex Spine")) { handler.Flexion(Data.getInstance()); }
        if (GUILayout.Button("Rewrite Extension")) { handler.WriteExtension(Data.getInstance()); }
        if (GUILayout.Button("Extend Spine")) { handler.Extension(Data.getInstance()); }
        if (GUILayout.Button("Rewrite Base")) { handler.WriteBase(Data.getInstance()); }
        if (GUILayout.Button("Reset Spine")) { handler.Base(Data.getInstance()); }
    }
}
