﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
public class EventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        if (target is not GameEvent e) return;
        if (GUILayout.Button("Raise"))
            e.Raise();
    }
}