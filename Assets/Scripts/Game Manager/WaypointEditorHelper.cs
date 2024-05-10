using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor(typeof(WaypointEditor))]
public class WaypointEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WaypointEditor waypointEditor = (WaypointEditor)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Waypoint"))
        {
            GameObject newWaypoint = waypointEditor.CreateWaypoint();
            Undo.RegisterCreatedObjectUndo(newWaypoint, "Add Waypoint");
        }

        if (GUILayout.Button("Clear Waypoints"))
        {
            Undo.RecordObject(waypointEditor, "Clear Waypoints");
            waypointEditor.ClearWaypoints();
        }
    }
}
