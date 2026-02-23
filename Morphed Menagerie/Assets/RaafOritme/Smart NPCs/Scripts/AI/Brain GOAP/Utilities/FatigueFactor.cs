namespace RaafOritme.SmartNPCs
{
    public class FatigueFactor : IUtilityFactor
    {
        public float weight = 0.5f;

        public float CalculateUtility(BrainGOAP _brainGoap, BaseAction _action)
        {
            float fatigue = _brainGoap.decisionSystem.utilities.fatigue; 
            float factor = 100f - fatigue;
            return factor * weight;
        }
    }
}
