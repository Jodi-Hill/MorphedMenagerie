using UnityEngine;

public class FloraMovement : MonoBehaviour
{
    public bool x, y, z;
    public float speed, offset;
    public bool randomX, randomY, randomZ;

    private float startx, starty, startz;

    public float val, timer;

    private void Start()
    {
        startx = randomX ? Random.Range(0,360) : 0;
        starty = randomY ? Random.Range(0,360) : 0;
        startz = randomZ ? Random.Range(0,360) : 0;
        timer = Random.Range(0, 1000);
    }

    void Update()
    {
        timer += Time.deltaTime;
        val = Mathf.Sin(timer * speed) * offset;
        float valx = x ? val : 0;
        float valy = y ? val : 0;
        float valz = z ? val : 0;

        transform.eulerAngles = new Vector3(valx + startx, valy + starty, valz + startz);
    }
}
