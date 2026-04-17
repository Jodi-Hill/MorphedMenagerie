using UnityEngine;

public class Dragging : MonoBehaviour
{
    [SerializeField] private bool isDragging = false;

    private float distanceFromCamera;
    private Plane dragPlane;
    private Vector3 startPosition;

    [SerializeField] private Transform[] snapPoints;
    [SerializeField] private float snapDistance = 0.5f;

    private void Start()
    {
        distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (dragPlane.Raycast(ray, out float distance))
            {
                transform.position = ray.GetPoint(distance);
            }
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        dragPlane = new Plane(Vector3.up, transform.position);
    }

    private void OnMouseUp()
    {
        isDragging = false;

        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform point in snapPoints)
        {
            float distance = Vector3.Distance(transform.position, point.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        if (closestPoint != null && closestDistance <= snapDistance)
        {
            transform.position = closestPoint.position;
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
