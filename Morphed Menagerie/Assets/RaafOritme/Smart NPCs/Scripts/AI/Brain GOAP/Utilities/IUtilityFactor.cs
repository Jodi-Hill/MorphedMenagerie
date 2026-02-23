namespace RaafOritme.SmartNPCs
{
    public interface IUtilityFactor
    {
        /// <summary>
        /// Calculates the current utility in order to decide what it should do with it.
        /// </summary>
        /// <param name="_brainGoap"></param>
        /// <param name="_action"></param>
        /// <returns></returns>
        public float CalculateUtility(BrainGOAP _brainGoap, BaseAction _action);
    }
}
