using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [CreateAssetMenu(fileName = "NPC Dialogue", menuName = "RaafOritme/Smart NPCs/Create Dialogue", order = 3)]
    [System.Serializable]
    public class DialogueSO : ScriptableObject
    {
        public string agentName = "NPC";
        public DialogueContainer dialogue;
    }

    [System.Serializable]
    public class DialogueContainer
    {
        /// <summary>
        /// This contains the text that the NPC for example asks the player, the player can choose up to 3 answers (supported visually by this asset pack).
        /// </summary>
        public string text;
        public DialogueAnswer[] answers;
    }
    
    [System.Serializable]
    public class DialogueAnswer
    {
        /// <summary>
        /// Each asnwer can trigger a new question or end the dialogue.
        /// </summary>
        public string answer;
        public DialogueContainer linksTo;
        public bool end;
    }
}
