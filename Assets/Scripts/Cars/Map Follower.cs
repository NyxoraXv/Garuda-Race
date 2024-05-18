using UnityEngine;

public class MinimapFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minFOV = 15f;
    [SerializeField] private float maxFOV = 60f;
    [SerializeField] private bool edgeScrolling = true;
    [SerializeField] private float edgeScrollThreshold = 20f;
    [SerializeField] private bool followRotation = true; // Follow target's rotation around itself?
    [SerializeField] private float distanceFromTarget = 15f; // Distance from target for orbiting
    [SerializeField] private Vector3 rotationOffset;

    private Camera minimapCamera;
    private float currentFOV;

    private void Start()
    {
        minimapCamera = GetComponent<Camera>();
        currentFOV = minimapCamera.fieldOfView;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Zooming (FOV for Perspective Camera)
            currentFOV -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);
            minimapCamera.fieldOfView = currentFOV;

            // Rotation Handling
            if (followRotation)
            {
                transform.RotateAround(target.position, Vector3.up, target.eulerAngles.y - transform.eulerAngles.y);
            }
            else
            {
                transform.RotateAround(target.position, Vector3.up, rotationOffset.y * Time.deltaTime);
            }

            // Calculate new position based on rotation and distance
            Vector3 desiredPosition = target.position + transform.rotation * new Vector3(0f, 0f, -distanceFromTarget);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // Adjust Height Based on FOV
            float height = 2f * Mathf.Tan(0.5f * currentFOV * Mathf.Deg2Rad) * distanceFromTarget;
            transform.position = new Vector3(transform.position.x, height, transform.position.z);

            // Edge Scrolling (if enabled)
            if (edgeScrolling)
            {
                HandleEdgeScrolling();
            }
        }
    }


    private void HandleEdgeScrolling()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 viewportPosition = minimapCamera.ScreenToViewportPoint(mousePosition);

        Vector3 moveDirection = Vector3.zero;

        if (viewportPosition.x < edgeScrollThreshold / Screen.width)
            moveDirection.x = -1f;
        else if (viewportPosition.x > 1f - (edgeScrollThreshold / Screen.width))
            moveDirection.x = 1f;

        if (viewportPosition.y < edgeScrollThreshold / Screen.height)
            moveDirection.z = -1f;
        else if (viewportPosition.y > 1f - (edgeScrollThreshold / Screen.height))
            moveDirection.z = 1f;

        transform.Translate(moveDirection * followSpeed * Time.deltaTime, Space.World);
    }
}
