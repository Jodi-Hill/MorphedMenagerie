namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// This class is completely empty in case that an entity has no states.
    /// This allows the use of FSM without the requirement for modules
    /// </summary>
    [System.Serializable]
    public class EmptyModule : BaseModule
    {
        public override void Initialize(AgentController _agentController)
        {
            agentController = _agentController;
        }

        public override void OnEnter(bool _excludeAction = false)
        {
        }

        public override void OnExit()
        {
        }

        public override void UpdateState(IBrain _brain)
        {
        }
    }
}
