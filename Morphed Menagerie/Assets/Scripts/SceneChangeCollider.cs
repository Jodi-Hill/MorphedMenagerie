using UnityEngine;

public class SceneChangeCollider : MonoBehaviour
{
    [SerializeField] private Act scene;
    [SerializeField] private GameObject text;

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            text.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        ActLoader.Instance.LoadAct(scene);
    }
}
