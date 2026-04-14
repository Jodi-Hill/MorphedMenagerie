using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private MonoBehaviour movementController;
    [SerializeField] private MonoBehaviour lookController;

    public Transform cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }

    public void OnDialogueStart()
    {
        movementController.enabled = false;
        lookController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnDialogueComplete()
    {
        movementController.enabled = true;
        lookController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
