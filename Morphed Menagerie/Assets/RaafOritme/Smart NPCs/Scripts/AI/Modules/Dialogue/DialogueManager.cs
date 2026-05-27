using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RaafOritme.SmartNPCs
{
    public class DialogueManager : MonoBehaviour
    {
        // Generic global settings
        private bool inDialogue;
        private DialogueContainer activeDialogue;
        private float textSpeed = 0;
        public static DialogueManager Instance { get; private set; }

        // TIP1: This system only supports up to 3 answers. By adding a ScrollRect, mask, and prefab you can dynamically create as many answers as you want with a scrollbar. 
        // TIP2: This pack does not ship TextMeshPro, however it is recommended to upgraded all UI elements to TMP. This gives you more freedom and flexibility when it comes to styling.
        // Dialogue settings
        [SerializeField] private GameObject dialogueCanvas;
        [SerializeField] private Text npcName;
        [SerializeField] private Text npcText;
        [SerializeField] private Text answerA;
        [SerializeField] private Text answerB;
        [SerializeField] private Text answerC;
        [SerializeField] private GameObject answerAObject;
        [SerializeField] private GameObject answerBObject;
        [SerializeField] private GameObject answerCObject;

        private void Awake()
        {
            Instance = this;
            // TIP: Usually classes like this have a destroy function here, but the object where this is assigned to, is already a SingleTon, so no need to add that here.
        }

        /// <summary>
        /// Triggered by systems such as the Dialogue Module.
        /// </summary>
        /// <param name="_dialogueInformation"></param>
        /// <param name="_textSpeed"></param>
        public void StartDialogue(DialogueSO _dialogueInformation, float _textSpeed)
        {
            if (inDialogue) return;
            inDialogue = true;

            activeDialogue = _dialogueInformation.dialogue;
            npcName.text = _dialogueInformation.agentName;
            textSpeed = _textSpeed;
            dialogueCanvas.SetActive(true);

            StartCoroutine(NextDialogue());
        }

        /// <summary>
        /// Shows dialogue through text animations. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator NextDialogue()
        {
            npcText.text = "";
            answerA.text = "";
            answerB.text = "";
            answerC.text = "";
            answerAObject.SetActive(false);
            answerBObject.SetActive(false);
            answerCObject.SetActive(false);
            yield return null;

            // Show text based on character speed. Zero means to show it instantly.
            if (textSpeed > 0)
            {
                float waitTime = textSpeed / activeDialogue.text.Length;
                foreach (char c in activeDialogue.text)
                {
                    npcText.text += c;
                    yield return new WaitForSeconds(waitTime);
                }
            }
            else
            {
                npcText.text = activeDialogue.text;
            }

            // Show all answers 1 by 1.
            int count = 0;
            foreach(DialogueAnswer answer in activeDialogue.answers)
            {
                yield return new WaitForSeconds(0.2f);

                // TIP: You can easily expand here to support more answers, or convert this to a dynamic solution.
                switch (count)
                {
                    case 0:
                        answerA.text = answer.answer;
                        answerAObject.SetActive(true);
                        break;
                    case 1:
                        answerB.text = answer.answer;
                        answerBObject.SetActive(true);
                        break;
                    case 2:
                        answerC.text = answer.answer;
                        answerCObject.SetActive(true);
                        break;
                    default:
                        Debug.LogWarning("This module only supports up to 3 answers! Please expand for more.");
                        break;
                }
                
                count++;
                if (count > 2)
                {
                    Debug.LogWarning("This module only supports up to 3 answers! Please expand for more.");
                    break;
                }
            }
        }

        /// <summary>
        /// Triggered by Unity Events through Canvas for example.
        /// </summary>
        /// <param name="answer">First answer is 0, second is 1, etc.</param>
        public void UpdateDialogue(int answer = 0)
        {
            if (activeDialogue.answers[answer].end || activeDialogue.answers[answer].linksTo == null)
            {
                dialogueCanvas.SetActive(false);
                inDialogue = false;
                return;
            }

            activeDialogue = activeDialogue.answers[answer].linksTo;
            StartCoroutine(NextDialogue());
        }
    }
}
