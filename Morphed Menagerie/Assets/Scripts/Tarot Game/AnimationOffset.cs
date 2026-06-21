using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().speed = Random.Range(0.9f, 1.1f);
    }
}
