using StarterAssets;
using UnityEngine;

public class CutsceneEnable : MonoBehaviour
{
    public GameObject cutScene;
    public GameObject player;
    public AnimationOnTrigger anim;
    public StarterAssetsInputs inputs;

    private bool cutsceneActive;

    public void StartCutscene()
    {
        cutScene.SetActive(true);
        player.SetActive(false);
        anim.Triggered();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !cutsceneActive)
        {
            cutsceneActive = true;
            StartCutscene();
        }
    }

    public void SwitchCams()
    {
        cutScene.SetActive(false);
        player.SetActive(true);
        inputs.move = Vector2.zero;
        inputs.look = Vector2.zero;
        inputs.jump = false;
        inputs.sprint = false;
    }
}

