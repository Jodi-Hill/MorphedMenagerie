using UnityEngine;
using UnityEngine.AI;

namespace RaafOritme.SmartNPCs
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CombatModule : BaseModule
    {
        private float aggressionCooldown;
        private float combatRange;
        private float executeTimer;
        private bool isInCombat;
        private bool isInRange;
        private Vector3 euler;
        internal Transform combatTarget;

        public Combat combatSettings;

        public override void Initialize(AgentController _agentController)
        {
            agentController = _agentController;
            combatSettings = agentController.settings.combat;
        }

        public override void OnEnter(bool _excludeAction = false)
        {
            aggressionCooldown = agentController.settings.combat.aggressionTime;
            combatRange = agentController.settings.combat.combatDistance;

            isInCombat = true;
        }

        public override void UpdateState(IBrain _brain)
        {
            // RO TODO implement combat -> This will be implemented in a future update.
            agentController.mainBrain.TransitionToNext();
            return;

            aggressionCooldown -= Time.deltaTime;

            // Face target
            euler = agentController.transform.eulerAngles;
            agentController.transform.LookAt(combatTarget.position);
            euler.y = agentController.transform.eulerAngles.y;
            agentController.transform.eulerAngles = euler;

            executeTimer -= Time.deltaTime;

            if (executeTimer <= 0.0f)
            {
                executeTimer = Random.Range(0, 5);
                combatSettings.actionDuringCombat.Invoke();
                // RO TODO attacking
            }

            // RO TODO can exit combat once health has run out from opponent
            if (aggressionCooldown <= 0/* || combatTarget.health <= 0*/)
            {
                agentController.SetOverrule(OverRuleState.NONE);
                // RO TODO transition to baseroutine
                agentController.mainBrain.TransitionToNext();
            }
        }

        /// <summary>
        /// Used as a callback method for when agent is forced into combat.
        /// </summary>
        public void EnterCombat(Transform _target)
        {
            if (agentController.GetOverrule() == OverRuleState.SCARED)
            {
                agentController.pathfinding.SetDestination(agentController.settings.idle.residence.position);
                return;
            }

            if (agentController.GetOverrule() == OverRuleState.DECEASED)
            {
                return;
            }

            agentController.mainBrain.TransitionToState(OverRuleState.COMBAT);
            agentController.SetOverrule(OverRuleState.COMBAT);
            combatTarget = _target;
            agentController.pathfinding.SetSpeed(agentController.settings.movement.runSpeed);
        }

        public override void OnExit()
        {
            combatSettings.actionAfterCombat.Invoke();
            isInCombat = false;
        }
    }
}
