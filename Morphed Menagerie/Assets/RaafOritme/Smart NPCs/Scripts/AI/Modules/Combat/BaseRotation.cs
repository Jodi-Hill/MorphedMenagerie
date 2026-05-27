namespace RaafOritme.SmartNPCs
{
    [System.Serializable]
    public abstract class BaseRotation
    {
        protected CombatModule agentModule;

        /// <summary>
        /// Initializes the rotation.
        /// </summary>
        /// <param name="_agentModule"></param>
        public abstract void Initialize(CombatModule _agentModule);

        /// <summary>
        /// Invoke the combat action.
        /// </summary>
        public abstract void Invoke();
    }
}
