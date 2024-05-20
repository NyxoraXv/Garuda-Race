using UnityEngine;

public class AICarController : MonoBehaviour
{
    public WaypointEditor waypointEditor;
    public float maxSpeed = 20f;
    public float rotationSpeed = 100f;
    public float acceleration = 5f;
    private const float ACCELERATION_BRAKING_RATIO = 2200f / 11f; // Your desired ratio

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public GameObject frontLeftWheelMesh;
    public GameObject frontRightWheelMesh;
    public GameObject rearLeftWheelMesh;
    public GameObject rearRightWheelMesh;
    public bool rotateWheelsWithSpeed = true; // Toggle in Inspector
    public float wheelRotationSpeedMultiplier = 6f;

    private int currentWaypointIndex = 0;
    private float targetSteerAngle = 0f;
    private float currentSpeed = 0f;

    public float startDelaySeconds = 3f; // Delay in seconds before the car starts moving
    private float elapsedTime = 0f;  // Track elapsed time
    private bool startedMoving = false; // Flag to indicate if the car has started


    void Start()
    {
        // Initialize car to look at the first waypoint
        if (waypointEditor.waypoints.Length > 0)
        {
            transform.LookAt(waypointEditor.waypoints[0].transform);
        }
    }

    void FixedUpdate()
    {
        if (!startedMoving)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= startDelaySeconds)
            {
                startedMoving = true;
            }
            else
            {
                return; // Exit FixedUpdate until the delay is over
            }
        }
        // Ensure waypoints exist before proceeding
        if (waypointEditor.waypoints == null || waypointEditor.waypoints.Length == 0)
            return;

        // Waypoint Navigation Logic
        GameObject currentWaypoint = waypointEditor.waypoints[currentWaypointIndex];
        Vector3 targetDirection = currentWaypoint.transform.position - transform.position;

        // Check if the car has reached the current waypoint
        if (targetDirection.magnitude < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointEditor.waypoints.Length;
        }

        // Dynamic Speed Adjustment
        float distanceToWaypoint = targetDirection.magnitude;
        float maxTurningSpeed = maxSpeed * 0.6f; // Maximum speed while turning sharply
        float targetSpeed = maxSpeed * (1f - Mathf.Clamp01(distanceToWaypoint / 20f));
        targetSpeed = Mathf.Clamp(targetSpeed, maxTurningSpeed, maxSpeed);

        // Calculate braking force based on the ratio
        float brakingForce = acceleration / ACCELERATION_BRAKING_RATIO;

        // Smooth acceleration and braking
        float accelerationRate = acceleration * Time.deltaTime;
        float brakingRate = brakingForce * Time.deltaTime;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed,
                                       currentSpeed < targetSpeed ? accelerationRate : brakingRate);

        // Steering Logic
        float steeringAngle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
        float speedSensitivity = Mathf.Lerp(1f, 0.5f, currentSpeed / maxSpeed); // Adjust steering sensitivity based on speed
        steeringAngle *= speedSensitivity;
        steeringAngle = Mathf.Clamp(steeringAngle / 2.5f, -45f, 45f);

        // Smooth steering
        targetSteerAngle = Mathf.Lerp(targetSteerAngle, steeringAngle, Time.deltaTime * rotationSpeed * 2f);
        frontLeftWheel.steerAngle = targetSteerAngle;
        frontRightWheel.steerAngle = targetSteerAngle;

        // Apply Motor Torque (using the adjusted currentSpeed)
        float movement = currentSpeed;
        frontLeftWheel.motorTorque = movement;
        frontRightWheel.motorTorque = movement;
        rearLeftWheel.motorTorque = movement;
        rearRightWheel.motorTorque = movement;

        if (rotateWheelsWithSpeed)
        {
            RotateWheelMeshBySpeed(frontLeftWheelMesh, currentSpeed);
            RotateWheelMeshBySpeed(frontRightWheelMesh, currentSpeed);
            RotateWheelMeshBySpeed(rearLeftWheelMesh, currentSpeed);
            RotateWheelMeshBySpeed(rearRightWheelMesh, currentSpeed);
        }
    }

    void RotateWheelMeshBySpeed(GameObject wheelMesh, float speed)
    {
        // Calculate rotation angle based on speed and time
        float rotationAngle = speed * wheelRotationSpeedMultiplier * Time.deltaTime;

        // Rotate the wheel mesh around its local X axis (forward)
        wheelMesh.transform.Rotate(Vector3.right, rotationAngle, Space.Self);
    }

}
