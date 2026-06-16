using UnityEngine;

public class StalkObject : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;

    private void Update()
    {
        transform.position = target.position + offset;
    }
}
