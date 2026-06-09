using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void StartScene()
    {
            SceneManager.LoadScene(sceneName);
    }
}
