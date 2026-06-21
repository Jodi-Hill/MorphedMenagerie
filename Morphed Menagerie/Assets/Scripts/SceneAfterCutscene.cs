using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnActivation : MonoBehaviour
{
    void OnEnable()
    {
        ActLoader.Instance.LoadAct(Act.FaoOutro);
    }
}