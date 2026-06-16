using UnityEngine;

public class CutsceneEnable : MonoBehaviour
{
    public GameObject cutScene;
    public Camera camera1;

    public void StartCutscene()
    {
        cutScene.SetActive(true);
        camera1.enabled = false;
    }
}

