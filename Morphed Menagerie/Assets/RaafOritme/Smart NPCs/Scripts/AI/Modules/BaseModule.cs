namespace RaafOritme.SmartNPCs
{
    [System.Serializable]
    public abstract class BaseModule
    {
        protected AgentController agentController;

        /// <summary>
        /// Initializes the module.
        /// </summary>
        /// <param name="_agentController"></param>
        public abstract void Initialize(AgentController _agentController);

        /// <summary>
        /// Entering the module means that it has to set certain settings.
        /// </summary>
        /// <param name="_excludeAction">If enabled it will not perform a specific action.</param>
        public abstract void OnEnter(bool _excludeAction = false);

        /// <summary>
        /// Ensure a smooth experience by using an exit method, can be used as a reset.
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// This will update a specific state.
        /// </summary>
        /// <param name="_brain"></param>
        public abstract void UpdateState(IBrain _brain);
    }
}
