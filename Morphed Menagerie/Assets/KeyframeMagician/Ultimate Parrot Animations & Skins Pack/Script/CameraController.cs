using UnityEngine;
using UnityEngine.EventSystems; // Required to check for UI interactions

namespace KeyframeMagician.UltimateParrotAnimsSkinsPack

{
    public class CameraController : MonoBehaviour
    {
        public Transform target; // The object to orbit around (e.g., the parrot model)
        public float rotationSpeed = 100f; // Speed of camera rotation
        public float zoomSpeed = 0.5f; // Speed of zooming with Right Mouse drag
        public float minDistance = 2f; // Minimum zoom distance
        public float maxDistance = 10f; // Maximum zoom distance

        private Vector3 initialOffset; // Initial offset from the target
        private float initialDistance; // Initial distance from the target
        private Quaternion initialRotation; // Initial rotation of the camera

        private Vector3 currentOffset; // Current offset from the target
        private float currentDistance; // Current distance from the target

        private float lastMouseY; // Tracks the last Y position of the mouse for drag zooming

        void Start()
        {
            // Calculate the initial offset and store the starting values
            initialOffset = transform.position - target.position;
            initialDistance = initialOffset.magnitude;
            initialRotation = transform.rotation;

            // Initialize current values
            currentOffset = initialOffset;
            currentDistance = initialDistance;

            // Ensure the camera is pointing at the target
            transform.LookAt(target);
        }

        void LateUpdate()
        {
            // Only allow camera controls if the pointer is not over a UI element
            if (!IsPointerOverUI())
            {
                // Rotate the camera when the left mouse button is pressed
                if (Input.GetMouseButton(0))
                {
                    float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                    float vertical = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

                    // Apply the rotation to the offset
                    Quaternion rotation = Quaternion.Euler(vertical, horizontal, 0);
                    currentOffset = rotation * currentOffset;
                }

                // Zoom the camera with Right Mouse drag
                if (Input.GetMouseButton(1)) // Right mouse button
                {
                    float currentMouseY = Input.mousePosition.y; // Get the current mouse Y position
                    if (lastMouseY != 0) // Ensure this isn't the first frame of the drag
                    {
                        float deltaY = currentMouseY - lastMouseY; // Inverted zoom behavior
                        currentDistance = Mathf.Clamp(currentDistance - deltaY * zoomSpeed * Time.deltaTime, minDistance, maxDistance);
                    }
                    lastMouseY = currentMouseY; // Update last mouse Y position
                }
                else
                {
                    lastMouseY = 0; // Reset lastMouseY when the right mouse button is released
                }
            }

            // Update the camera's position and maintain the current offset
            transform.position = target.position + currentOffset.normalized * currentDistance;

            // Keep the camera looking at the target
            transform.LookAt(target);

            // Reset camera position and rotation when the "R" key is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetCamera();
            }
        }

        /// <summary>
        /// Resets the camera to its initial position, rotation, and distance.
        /// </summary>
        void ResetCamera()
        {
            currentOffset = initialOffset;
            currentDistance = initialDistance;
            transform.position = target.position + initialOffset;
            transform.rotation = initialRotation;

            Debug.Log("Camera reset to initial position and rotation.");
        }

        /// <summary>
        /// Checks if the pointer is currently over a UI element.
        /// </summary>
        /// <returns>True if the pointer is over a UI element, false otherwise.</returns>
        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}
