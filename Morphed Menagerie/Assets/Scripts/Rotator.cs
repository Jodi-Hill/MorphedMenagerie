using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool x, y, z;
    public float speed = 5;
    
    void Update()
    {
        transform.eulerAngles += new Vector3(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0) * speed * Time.deltaTime;
    }
}
