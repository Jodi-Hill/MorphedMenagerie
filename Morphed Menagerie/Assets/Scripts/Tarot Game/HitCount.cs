using UnityEngine;
using TMPro;

public class HitCount : MonoBehaviour
{
    public TextMeshPro text;
    public Vector3 startScale;
    public Vector3 endScale;
    public float timeEffect = 1f;

    private void Start()
    {
        startScale = transform.localScale;
        endScale = transform.localScale * 2f;
    }

    private void Update()
    {
        if (timeEffect > 0)
        {
            timeEffect -= Time.deltaTime * 2f;
            transform.localScale = Vector3.Lerp(endScale, startScale, timeEffect); // reversed because timer goes from 1 to 0
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
