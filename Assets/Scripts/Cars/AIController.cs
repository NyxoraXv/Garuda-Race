using UnityEngine;

public class AIController : MonoBehaviour
{
    public WaypointEditor waypointEditor;
    public float speed = 5.0f;
    public float minAccuracy = 0.5f;
    public float maxAccuracy = 1.5f;
    public float rotSpeed = 5.0f;
    public float turnSpeed = 5.0f;
    public float offsetRandomness = 0.5f;

    private int currentWaypoint = 0;
    private Transform[] waypoints;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (waypointEditor != null && waypointEditor.waypoints.Length > 0)
        {
            waypoints = new Transform[waypointEditor.waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = waypointEditor.waypoints[i].transform;
            }

            waypoints[0].position = rb.position;

            Debug.Log("Waypoints count: " + waypoints.Length);
            Debug.Log("rb: " + rb);
        }
    }


    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Vector3 direction = waypoints[currentWaypoint].position - rb.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotSpeed);
        }

        float distance = Vector3.Distance(waypoints[currentWaypoint].position, rb.position);
        float randomizedAccuracy = Random.Range(minAccuracy, maxAccuracy);

        if (distance < randomizedAccuracy)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 1;
            }
        }

        Vector3 randomOffset = new Vector3(Random.Range(-offsetRandomness, offsetRandomness), 0f, Random.Range(-offsetRandomness, offsetRandomness));
        Vector3 targetPosition = waypoints[currentWaypoint].position + randomOffset;

        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, Time.deltaTime * speed));
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        UnityEditor.Handles.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] != null)
            {
                if (i + 1 < waypoints.Length && waypoints[i + 1] != null)
                {
                    UnityEditor.Handles.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                if (i == waypoints.Length - 1 && waypoints[0] != null)
                {
                    UnityEditor.Handles.DrawLine(waypoints[i].position, waypoints[0].position);
                }
            }
        }
    }
#endif
}
