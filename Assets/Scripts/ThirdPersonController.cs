using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign your Main Camera here (or it will auto-grab Camera.main at runtime).")]
    public Transform cameraTransform;

    [Header("Movement")]
    [Tooltip("WASD movement speed (m/s).")]
    public float movementSpeed = 5f;
    [Tooltip("Mouse X sensitivity for yaw rotation.")]
    public float mouseSensitivity = 200f;
    [Tooltip("Gravity applied to the controller (negative value).")]
    public float gravity = -9.81f;

    [Header("Camera")]
    [Tooltip("Default camera offset relative to the player. Y = height, Z = distance (use negative to sit behind).")]
    public Vector3 cameraOffset = new Vector3(0f, 2f, -4f);
    [Tooltip("How quickly the camera follows to its target position.")]
    public float cameraPitch = -10f;
    [Tooltip("How quickly the camera follows to its target position.")]
    public float cameraFollowDamping = 10f;
    [Tooltip("Lock and hide the cursor for mouse-look.")]
    public bool lockCursor = true;

    private CharacterController controller;
    private float yaw;                // current yaw around Y axis (in degrees)
    private Vector3 verticalVelocity; // only Y is used

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Auto-assign main camera if not set
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {
        // Start yaw from the player's current Y rotation
        yaw = transform.eulerAngles.y;

        // Snap camera to its initial desired spot if we have one
        if (cameraTransform != null)
        {
            Vector3 rotatedOffset = Quaternion.Euler(0f, yaw, 0f) * cameraOffset;
            cameraTransform.position = transform.position + rotatedOffset;
            cameraTransform.rotation = Quaternion.LookRotation(
                (transform.position + Vector3.up * cameraOffset.y) - cameraTransform.position
            ) * Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    void Update()
    {
        HandleMouseLookYawOnly();
        HandleMovementWASD();
        HandleCameraFollow();
    }

    private void HandleMouseLookYawOnly()
    {
        // Horizontal mouse moves only rotate around Y (no pitch)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yaw += mouseX;

        // Rotate the player (pivot) around Y
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private void HandleMovementWASD()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D, left/right
        float v = Input.GetAxisRaw("Vertical");   // W/S, forward/back

        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        // Move relative to the player's facing (yaw)
        Vector3 worldMove = transform.TransformDirection(input) * movementSpeed;

        // Simple gravity
        if (controller.isGrounded && verticalVelocity.y < 0f)
            verticalVelocity.y = -2f; // small stick-to-ground value

        verticalVelocity.y += gravity * Time.deltaTime;

        // Apply horizontal then vertical motion
        controller.Move(worldMove * Time.deltaTime);
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private void HandleCameraFollow()
    {
        if (cameraTransform == null) return;

        // Keep the camera orbiting around the player using yaw only
        Vector3 rotatedOffset = Quaternion.Euler(0f, yaw, 0f) * cameraOffset;
        Vector3 targetPos = transform.position + rotatedOffset;

        // Smooth follow
        float t = 1f - Mathf.Exp(-cameraFollowDamping * Time.deltaTime);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, t);

        // Look at player and apply fixed pitch tilt
        Vector3 lookTarget = transform.position + Vector3.up * cameraOffset.y;
        cameraTransform.rotation = Quaternion.LookRotation(lookTarget - cameraTransform.position);
        cameraTransform.rotation *= Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void OnDisable()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
