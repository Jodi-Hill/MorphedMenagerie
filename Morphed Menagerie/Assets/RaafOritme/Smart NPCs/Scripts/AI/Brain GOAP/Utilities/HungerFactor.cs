namespace RaafOritme.SmartNPCs
{
    public class HungerFactor : IUtilityFactor
    {
        public float weight = 0.25f;

        public float CalculateUtility(BrainGOAP _brainGoap, BaseAction _action)
        {
            float fatigue = _brainGoap.decisionSystem.utilities.hunger;
            float utility = 100f - fatigue;
            return utility * weight;
        }
    }
}
