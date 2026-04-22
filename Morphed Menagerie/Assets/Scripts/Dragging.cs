using UnityEngine;

public class Dragging : MonoBehaviour
{
    [SerializeField] private bool isDragging = false;

    private float distanceFromCamera;
    private Plane dragPlane;
    private Vector3 startPosition;

    [SerializeField] private SnapPoint[] snapPoints;
    [SerializeField] private float snapDistance = 0.5f;

    private SnapPoint currentSnapPoint;


    public CardManager cardManager;
    public CardData cardData;


    private void OnMouseDown()
    {
        cardManager.SetPlayerCard(cardData);
    }


    /*
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

        if (currentSnapPoint != null)
        {
            currentSnapPoint.isOccupied = false;
            currentSnapPoint = null;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        SnapPoint closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (SnapPoint point in snapPoints)
        {
            if (point.isOccupied) continue;

            float distance = Vector3.Distance(transform.position, point.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        if (closestPoint != null && closestDistance <= snapDistance)
        {
            if (currentSnapPoint != null)
            {
                currentSnapPoint.isOccupied = false;
            }
            transform.position = closestPoint.transform.position;
            closestPoint.isOccupied = true;
            currentSnapPoint = closestPoint;
        }
        else
        {
            transform.position = startPosition;
        }
    }*/
}
