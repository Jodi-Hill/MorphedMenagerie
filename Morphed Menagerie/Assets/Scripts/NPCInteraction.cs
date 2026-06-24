using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string nodeName = "Start";
    [SerializeField] private GameObject text;

    public UnityEventString nodeStartEvents, nodeComplete;
    public UnityEvent dialogueStart, dialogueComplete;

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            text.SetActive(true);
            Debug.Log("Press E to talk");
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
        if (playerInside && Input.GetKeyDown(KeyCode.E) && !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.onNodeStart = nodeStartEvents;
            dialogueRunner.onNodeComplete = nodeComplete;
            dialogueRunner.onDialogueStart = dialogueStart;
            dialogueRunner.onDialogueComplete = dialogueComplete;
            dialogueRunner.StartDialogue(nodeName);
        }
    }
}
