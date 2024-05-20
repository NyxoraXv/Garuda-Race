using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class MachineLearningAIController : Agent
{
    // Car Components
    public WheelCollider[] wheelColliders;
    public Rigidbody rb;

    // Sensors and Perception
    public float raycastDistance = 50f;
    public LayerMask obstacleLayer;

    // Waypoint Tracking
    public WaypointEditor waypointEditor;
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    // Control Parameters
    public float maxSteeringAngle = 30f;
    public float motorTorque = 150f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 30f;
    public float idleTimeBeforeReset = 3f; // Time to wait if stuck
    private float stuckTimer = 0f;

    // ML-Agents Methods
    public override void CollectObservations(VectorSensor sensor)
    {
        // Car State
        sensor.AddObservation(transform.InverseTransformDirection(rb.velocity)); // Local velocity
        sensor.AddObservation(transform.rotation.eulerAngles.y); // Car rotation

        // Wheel Information
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            sensor.AddObservation(wheelCollider.rpm);
        }

        // Distance and Direction to Next Waypoint
        Vector3 toWaypoint = waypoints[currentWaypointIndex].position - transform.position;
        sensor.AddObservation(toWaypoint.magnitude);
        sensor.AddObservation(Vector3.SignedAngle(transform.forward, toWaypoint, Vector3.up));

        // Obstacle Detection (Raycast)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, obstacleLayer))
        {
            sensor.AddObservation(hit.distance / raycastDistance); // Normalized distance
        }
        else
        {
            sensor.AddObservation(1f); // No obstacle detected
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Interpret Actions
        float steering = actions.ContinuousActions[0];
        float throttleBrake = actions.ContinuousActions[1];

        // Apply Actions to Car
        ApplySteering(steering);
        ApplyThrottleBrake(throttleBrake);

        // Reward Calculation
        float speed = rb.velocity.magnitude;
        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

        AddReward(-distanceToWaypoint / 1000f); // Encourage reaching waypoints
        AddReward(speed / maxSpeed);             // Encourage speed (up to a limit)
        AddReward(-Mathf.Abs(steering) / maxSteeringAngle); // Encourage smooth steering

        // Waypoint Progress and Termination
        if (distanceToWaypoint < 5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            AddReward(1f);
        }

        // Detect if the car is stuck
        if (speed < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= idleTimeBeforeReset)
            {
                AddReward(-1f); // Penalize for being stuck
                EndEpisode();
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    // Helper Methods
    private void ApplySteering(float steeringInput)
    {
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            wheelCollider.steerAngle = steeringInput * maxSteeringAngle;
        }
    }

    private void ApplyThrottleBrake(float throttleBrakeInput)
    {
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            if (throttleBrakeInput > 0)
            {
                wheelCollider.motorTorque = throttleBrakeInput * motorTorque;
                wheelCollider.brakeTorque = 0;
            }
            else
            {
                wheelCollider.motorTorque = 0;
                wheelCollider.brakeTorque = -throttleBrakeInput * brakeTorque;
            }
        }
    }
    // Initialization
    public override void OnEpisodeBegin()
    {
        if (waypointEditor != null)
        {
            waypoints = new List<Transform>(waypointEditor.waypoints.Length);
            foreach (GameObject waypoint in waypointEditor.waypoints)
            {
                waypoints.Add(waypoint.transform);
            }
        }
        else
        {
            Debug.LogError("WaypointEditor not assigned to CarAgent!");
        }

        // Reset car and waypoint index
        ResetCar();
        currentWaypointIndex = 0;
    }

    private void ResetCar()
    {
        // Randomize starting position within a defined area
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
        transform.position = randomPosition;

        // Reset car's rotation
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

        // Reset car's velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Reset wheel colliders
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            wheelCollider.motorTorque = 0;
            wheelCollider.brakeTorque = Mathf.Infinity; // Full brakes
        }
    }

    // Heuristic (Optional - for Manual Control during Testing)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");  // Steering
        continuousActionsOut[1] = Input.GetAxis("Vertical");    // Throttle/Brake
    }

    // Collision Handling (Optional - for additional rewards/penalties)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f); // Penalty for hitting a wall
            EndEpisode();
        }
    }
}