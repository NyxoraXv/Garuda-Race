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
    private WheelCollider[] wheelColliders;

    void Start()
    {
        if (waypointEditor != null && waypointEditor.waypoints.Length > 0)
        {
            waypoints = new Transform[waypointEditor.waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = waypointEditor.waypoints[i].transform;
            }
            Debug.Log("Waypoints count: " + waypoints.Length);
        }

        wheelColliders = GetComponentsInChildren<WheelCollider>();
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Vector3 direction = waypoints[currentWaypoint].position - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        }

        float distance = Vector3.Distance(waypoints[currentWaypoint].position, transform.position);

        if (distance < Random.Range(minAccuracy, maxAccuracy))
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0; // Reset to the first waypoint
            }
        }

        Vector3 randomOffset = new Vector3(Random.Range(-offsetRandomness, offsetRandomness), 0f, Random.Range(-offsetRandomness, offsetRandomness));
        Vector3 targetPosition = waypoints[currentWaypoint].position + randomOffset;

        MoveCar(targetPosition);
    }

    void MoveCar(Vector3 targetPosition)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float targetSpeed = Mathf.Clamp01(distanceToTarget / maxAccuracy) * speed;

        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.motorTorque = targetSpeed;
            wheel.steerAngle = CalculateSteerAngle(targetPosition);
        }
    }

    float CalculateSteerAngle(Vector3 targetPosition)
    {
        Vector3 targetDirection = targetPosition - transform.position;
        Vector3 localTarget = transform.InverseTransformPoint(targetPosition);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float angle = Mathf.Clamp(targetAngle, -turnSpeed, turnSpeed);

        return angle;
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
