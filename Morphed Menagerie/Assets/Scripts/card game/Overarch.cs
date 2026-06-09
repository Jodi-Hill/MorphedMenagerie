using UnityEngine;
using UnityEngine.SceneManagement;

public class Overarch : MonoBehaviour
{
    public string menu, zoo1, rini, rinijoin, zoo2, fao, faosacri, outro;
    public static Overarch Instance { get; private set; }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(menu);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(menu);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(zoo1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(rini);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(rinijoin);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(zoo2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            SceneManager.LoadScene(fao);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            SceneManager.LoadScene(faosacri);
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            SceneManager.LoadScene(outro);
        }
    }
}
