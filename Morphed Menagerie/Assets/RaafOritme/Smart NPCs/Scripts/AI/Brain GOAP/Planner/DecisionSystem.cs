using System.Collections.Generic;

namespace RaafOritme.SmartNPCs
{
    public class DecisionSystem
    {
        public Utilities utilities;
        public UtilitySystem utilitySystem;
        public EmotionSystem emotionSystem;
        public ActionPlanner actionPlanner;
        public BrainGOAP brainGoap;

        public DecisionSystem (BrainGOAP _brainGoap)
        {
            brainGoap = _brainGoap;
            utilities = new();
            actionPlanner = new();
            utilitySystem = new(_brainGoap);
            emotionSystem = new(_brainGoap);

            // TIP: Make a npc more unique by having various factors or combinations of factors.
            utilitySystem.AddFactor(new EmotionFactor(emotionSystem));
            utilitySystem.AddFactor(new FatigueFactor());
            utilitySystem.AddFactor(new HungerFactor());
        }

        /// <summary>
        /// Updates all subsystems.
        /// </summary>
        public void UpdateSystems()
        {
            utilities.UpdateNeeds();
            emotionSystem.UpdateEmotion();
        }

        /// <summary>
        /// Returns a chosen action as a decision that it will have to make based on various factors.
        /// </summary>
        /// <returns></returns>
        public BaseAction MakeDecision()
        {
            // TIP: A search filter can be added in order to check for certain conditions before building the actions stack.
            List<BaseAction> availableActions = actionPlanner.ScanForActions(brainGoap);
            Dictionary<BaseAction, float> actionUtilities = utilitySystem.CalculateUtilities(availableActions);
            BaseAction bestAction = actionPlanner.SelectBestAction(brainGoap, actionUtilities);

            return bestAction;
        }
    }
}
