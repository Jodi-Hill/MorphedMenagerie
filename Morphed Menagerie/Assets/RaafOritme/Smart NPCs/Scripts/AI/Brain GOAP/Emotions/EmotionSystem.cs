using System.Collections.Generic;

namespace RaafOritme.SmartNPCs
{
    public class EmotionSystem
    {
        public BrainGOAP brainGoap;
        public Personality personality;
        public EmotionalState emotionalState;
        public EmotionalMemory emotionalMemory;

        public EmotionState CurrentEmotion { get; private set; }
        // Dictionary to hold modifiers based on current emotion
        private Dictionary<ActionType, float> emotionModifiers = new Dictionary<ActionType, float>();

        public EmotionSystem(BrainGOAP _brainGoap)
        {
            brainGoap = _brainGoap;
            personality = brainGoap.personality;
            emotionalState = new();
            emotionalMemory = new();
        }

        /// <summary>
        /// Updates all emotion related systems and modifiers.
        /// </summary>
        public void UpdateEmotion()
        {
            // TIP: Update current emotion based on events, triggers, etc.
            emotionalState.Update();
            UpdateModifiers();
        }

        private void UpdateModifiers()
        {
            emotionModifiers.Clear();

            switch (CurrentEmotion)
            {
                case EmotionState.Angry:
                    emotionModifiers[ActionType.Aggressive] = 1.5f;
                    emotionModifiers[ActionType.Passive] = 0.5f;
                    break;
                case EmotionState.Happy:
                    emotionModifiers[ActionType.Social] = 1.2f;
                    emotionModifiers[ActionType.Aggressive] = 0.8f;
                    break;
                // TIP: Add more emotions and their modifiers
                default:
                    // Neutral modifiers
                    emotionModifiers[ActionType.Aggressive] = 1f;
                    emotionModifiers[ActionType.Passive] = 1f;
                    break;
            }
        }

        /// <summary>
        /// Convert base action to an emotion tied parameter in order to get the modifier.
        /// </summary>
        /// <param name="_action"></param>
        /// <returns></returns>
        public float GetEmotionModifier(BaseAction _action)
        {
            ActionType actionType = ActionType.Social;

            switch(_action)
            {
                case PatrolAction ac:
                    actionType = ActionType.Aggressive;
                    break;
                case SeekShelterAction ac:
                    actionType = ActionType.Passive;
                    break;
            }

            if (emotionModifiers.ContainsKey(actionType))
            {
                return emotionModifiers[actionType];
            }
            return 1f; // Default modifier
        }
    }
}
