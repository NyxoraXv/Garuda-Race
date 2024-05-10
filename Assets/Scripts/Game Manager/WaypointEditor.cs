using UnityEngine;

public class WaypointEditor : MonoBehaviour
{
    public GameObject[] waypoints;

    public GameObject CreateWaypoint()
    {
        Vector3 position = Vector3.zero; // Default position
        if (waypoints != null && waypoints.Length > 0 && waypoints[waypoints.Length - 1] != null)
        {
            position = waypoints[waypoints.Length - 1].transform.position;
        }

        GameObject waypoint = new GameObject("Waypoint");
        waypoint.transform.position = position;
        waypoint.transform.SetParent(transform); // Optional: Set parent to organize waypoints under a common GameObject.
        AddWaypoint(waypoint);
        ConnectWaypoints(); // Connect the last waypoint to the first to make it circular
        return waypoint;
    }

    void AddWaypoint(GameObject waypoint)
    {
        if (waypoints == null)
        {
            waypoints = new GameObject[] { waypoint };
        }
        else
        {
            GameObject[] newArray = new GameObject[waypoints.Length + 1];
            for (int i = 0; i < waypoints.Length; i++)
            {
                newArray[i] = waypoints[i];
            }
            newArray[newArray.Length - 1] = waypoint;
            waypoints = newArray;
        }
    }

    void ConnectWaypoints()
    {
        if (waypoints != null && waypoints.Length > 1 && waypoints[0] != null && waypoints[waypoints.Length - 1] != null)
        {
            // Connect the last waypoint to the first one
            waypoints[waypoints.Length - 1].transform.position = waypoints[0].transform.position;
        }
    }

    public void ClearWaypoints()
    {
        waypoints = new GameObject[0];
    }

#if UNITY_EDITOR
    // Draw waypoints in the editor for visual debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawSphere(waypoints[i].transform.position, 0.5f);
                }
            }

            DrawWaypointLines();
        }
    }

    void DrawWaypointLines()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                if (waypoints[i] != null && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                }
            }
            // Draw line between the last and first waypoint to make it circular
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].transform.position, waypoints[0].transform.position);
        }
    }
#endif
}