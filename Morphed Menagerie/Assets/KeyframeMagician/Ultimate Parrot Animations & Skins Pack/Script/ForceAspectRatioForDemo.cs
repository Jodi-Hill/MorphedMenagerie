using UnityEngine;
using UnityEngine.SceneManagement; // Required to check the current scene

namespace KeyframeMagician.UltimateParrotAnimsSkinsPack

{
    public class ForceAspectRatioForDemo : MonoBehaviour
    {
        public string demoSceneName = "DemoScene"; // Replace with your demo scene's name
        public float targetAspectRatio = 16f / 9f; // Target aspect ratio (16:9)

        private Camera mainCamera;

        void Start()
        {
            // Check if the current scene matches the demo scene name
            if (SceneManager.GetActiveScene().name == demoSceneName)
            {
                // Get the main camera
                mainCamera = Camera.main;

                if (mainCamera != null)
                {
                    // Enforce the target aspect ratio
                    AdjustCameraViewport();
                }
                else
                {
                    Debug.LogError("Main camera not found. Please ensure there's a camera tagged as 'MainCamera' in the scene.");
                }
            }
        }

        void AdjustCameraViewport()
        {
            // Get the current screen aspect ratio
            float windowAspectRatio = (float)Screen.width / (float)Screen.height;

            // Calculate scale height based on the target aspect ratio
            float scaleHeight = windowAspectRatio / targetAspectRatio;

            if (scaleHeight < 1.0f)
            {
                // Add letterboxing (black bars at the top and bottom)
                Rect rect = mainCamera.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                mainCamera.rect = rect;
            }
            else
            {
                // Add pillarboxing (black bars at the sides)
                float scaleWidth = 1.0f / scaleHeight;

                Rect rect = mainCamera.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                mainCamera.rect = rect;
            }
        }

        void OnPreCull()
        {
            // Clear the background behind the letterbox or pillarbox areas
            if (mainCamera != null)
            {
                GL.Clear(true, true, Color.black);
            }
        }
    }
}
