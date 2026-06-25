using System.Collections;
using UnityEngine;

public class DisableLoader : MonoBehaviour
{
    public float time = 1f;

    void Start()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(time);
        ActLoader.Instance.DisableLoading(); 
    }
}
