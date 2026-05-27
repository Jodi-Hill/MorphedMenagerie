namespace RaafOritme.SmartNPCs
{
    public class DialogueModule : BaseModule
    {
        public override void Initialize(AgentController _agentController)
        {
            agentController = _agentController;
        }

        public override void OnEnter(bool _excludeAction = false)
        {
            DialogueManager.Instance.StartDialogue(agentController.settings.dialogue.dialogue, agentController.settings.dialogue.textSpeed);
        }

        public override void OnExit()
        {
        }

        public override void UpdateState(IBrain _brain)
        {
        }
    }
}
