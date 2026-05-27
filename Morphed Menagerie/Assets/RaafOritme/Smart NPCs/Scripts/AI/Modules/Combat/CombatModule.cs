using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RaafOritme.SmartNPCs
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CombatModule : BaseModule
    {
        private float aggressionCooldown;
        private float combatRange; // TIP: can be used to determine how close the agent should be for combat
        private float executeTimer;
        private Vector3 euler;
        internal Transform combatTarget;

        public Combat combatSettings;
        private Queue<CombatRotation> combatRotations;
        private Queue<CombatContainer> combatContainers = new();
        public CombatRotation currentRotation;

        public override void Initialize(AgentController _agentController)
        {
            agentController = _agentController;
            combatSettings = agentController.settings.combat;
            combatRotations = new Queue<CombatRotation>(combatSettings.combatRotations);
            foreach (CombatRotation rotation in combatRotations)
            {
                foreach (CombatContainer container in rotation.combatRotations)
                {
                    container.combat.Initialize(this);
                }
            }
            QueueAttacks();
        }

        public override void OnEnter(bool _excludeAction = false)
        {
            aggressionCooldown = agentController.settings.combat.aggressionTime;
            combatRange = agentController.settings.combat.combatDistance;

            if (agentController.GetOverrule() == OverRuleState.SCARED)
            {
                agentController.pathfinding.SetDestination(agentController.settings.idle.residence.position);
                return;
            }

            if (agentController.GetOverrule() == OverRuleState.DECEASED)
            {
                return;
            }

            combatTarget = agentController.attacker;
            agentController.attacker = null; // TIP: This ensures that the agent will only be temporarily agressive when attacked.
            agentController.pathfinding.SetSpeed(agentController.settings.movement.runSpeed);
        }

        public override void UpdateState(IBrain _brain)
        {
            aggressionCooldown -= Time.deltaTime;

            if (combatTarget == null)
            {
                agentController.mainBrain.TransitionToNext();
            }

            // Face target
            euler = agentController.transform.eulerAngles;
            agentController.transform.LookAt(combatTarget.position);
            euler.y = agentController.transform.eulerAngles.y;
            agentController.transform.eulerAngles = euler;

            executeTimer -= Time.deltaTime;

            if (executeTimer <= 0.0f)
            {
                executeTimer = Random.Range(1, 5);
                combatSettings.actionDuringCombat.Invoke();
                if (combatContainers.Count == 0)
                {
                    QueueAttacks();
                }
                CombatContainer attack = combatContainers.Dequeue();
                attack.combat.Invoke();
            }

            // TIP: can exit combat once health has run out from opponent
            if (aggressionCooldown <= 0/* || combatTarget.health <= 0*/)
            {
                agentController.SetOverrule(OverRuleState.NONE);
                agentController.mainBrain.TransitionToNext();
            }
        }

        /// <summary>
        /// Invokes actions after combat.
        /// </summary>
        public override void OnExit()
        {
            combatSettings.actionAfterCombat.Invoke();
        }

        /// <summary>
        /// Build the next queue of attacks based on the first rotation in line.
        /// </summary>
        private void QueueAttacks()
        {
            currentRotation = combatRotations.Dequeue();
            combatContainers.Clear();
            foreach(CombatContainer container in currentRotation.combatRotations)
            {
                combatContainers.Enqueue(container);
            }
            combatRotations.Enqueue(currentRotation);
        }
    }
}
