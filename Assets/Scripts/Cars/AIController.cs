using UnityEngine;

public class AICarController : MonoBehaviour
{
    public WaypointEditor waypointEditor;
    public float maxSpeed = 20f;
    public float rotationSpeed = 100f;
    public float acceleration = 5f;
    public float brakingForce = 10f;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    private int currentWaypointIndex = 0;
    private float targetSteerAngle = 0f;
    private float currentSpeed = 0f;

    void Start()
    {
        Time.timeScale = 20f;
        if (waypointEditor.waypoints.Length > 0)
        {
            transform.LookAt(waypointEditor.waypoints[0].transform);
        }
    }

    void FixedUpdate()
    {
        if (waypointEditor.waypoints == null || waypointEditor.waypoints.Length == 0)
            return;

        GameObject currentWaypoint = waypointEditor.waypoints[currentWaypointIndex];
        Vector3 targetDirection = currentWaypoint.transform.position - transform.position;

        if (targetDirection.magnitude < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointEditor.waypoints.Length;
        }

        // Steering Logic
        float steeringAngle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

        // Adjust sensitivity based on speed (higher sensitivity at lower speeds)
        float speedSensitivity = Mathf.Lerp(1f, 0.5f, currentSpeed / maxSpeed); // Adjust lerp values as needed
        steeringAngle *= speedSensitivity;

        steeringAngle = Mathf.Clamp(steeringAngle / 2.5f, -45f, 45f); // Increased divisor for easier turning
        targetSteerAngle = Mathf.Lerp(targetSteerAngle, steeringAngle, Time.deltaTime * rotationSpeed * 2f); // Increased lerp speed
        frontLeftWheel.steerAngle = targetSteerAngle;
        frontRightWheel.steerAngle = targetSteerAngle;
        // Acceleration Logic
        float targetSpeed = maxSpeed;

        // Adjust target speed based on the steering angle
        if (Mathf.Abs(targetSteerAngle) > 10f)
        {
            targetSpeed = maxSpeed * (1 - (Mathf.Abs(targetSteerAngle) / 45f));
        }

        if (currentSpeed < targetSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed -= brakingForce * Time.deltaTime;
        }

        // Apply motor torque based on current speed
        float movement = currentSpeed;
        frontLeftWheel.motorTorque = movement;
        frontRightWheel.motorTorque = movement;
        rearLeftWheel.motorTorque = movement;
        rearRightWheel.motorTorque = movement;
    }
}
