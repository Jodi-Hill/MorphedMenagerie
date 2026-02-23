using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// This performance brain is automatically used by all NPCs in order to maintain a high performance.
    /// </summary>
    public class PerformanceBrain : AbstractBrain
    {
        private Vector3 targetPosition = Vector3.zero;
        private int waypointIndex = 0;

        public override void Initialize(AIRoutine _routine, GameObject _gameObject, AgentController _controller) { }
        public override void Start() { }
        public override void TransitionToNext() { }
        public override void TransitionToState(OverRuleState _state) { }

        // TIP: An OnEnter method can be introduced where the agent fetches the most nearby patrol area and patrol node to start patrolling from.

        public override void Update()
        {
            if (agentController.settings.patrol.patrolAreas.Count == 0 ||
                agentController.settings.patrol.patrolAreas[0].nodes.Count == 0)
            {
                // The agent doesn't have a patrol area that it can patrol at, so it will just remain frozen.
                return;
            }

            if (agentController.transform.SquaredDistance(targetPosition) < 4 || targetPosition == Vector3.zero)
            {
                waypointIndex++;
                if (waypointIndex >= agentController.settings.patrol.patrolAreas[0].nodes.Count)
                {
                    waypointIndex = 0;
                }
                targetPosition = agentController.settings.patrol.patrolAreas[0].nodes[waypointIndex].transform.position;
            }
            agentController.transform.position = Vector3.MoveTowards(agentController.transform.position, targetPosition, .025f);
        }
    }
}
