using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private DialogueRunner dialogueRunner;

    void Start()
    {
        dialogueRunner.AddCommandHandler<string>("load_scene", (sceneName) =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}
