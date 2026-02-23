namespace RaafOritme.SmartNPCs
{
    public class EmotionFactor : IUtilityFactor
    {
        private EmotionSystem emotionSystem;

        public EmotionFactor(EmotionSystem emotionSystem)
        {
            this.emotionSystem = emotionSystem;
        }

        public float CalculateUtility(BrainGOAP _brainGoap, BaseAction _action)
        {
            // Modify utility based on emotional state
            // For example, an "angry" NPC might have higher utility for aggressive actions
            float modifier = emotionSystem.GetEmotionModifier(_action);
            return modifier;
        }
    }
}
