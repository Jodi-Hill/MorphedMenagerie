using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class PatrolModule : BaseModule
    {
        private int nodesLeft;
        private Queue<PatrolNode> nodes = new(0);
        private PatrolNode target;
        private PatrolNode priorityTarget;
        private PatrolNode prevPrioritiyTarget;
        private Vector3 targetDestination;

        private bool runningRoutine;

        private Transform lookAtTarget;

        public Patrol patrolSettings;

        public override void Initialize(AgentController _agentController)
        {
            agentController = _agentController;
            patrolSettings = agentController.settings.patrol;
            patrolSettings.patrolAreaIndex = 0;
        }

        /// <summary>
        /// Sets a patrol area.
        /// </summary>
        /// <param name="_patrolArea"></param>
        public void SetPatrolInfo(PatrolArea _patrolArea)
        {
            patrolSettings.patrolAreas.Clear();
            patrolSettings.patrolAreas.Add(_patrolArea);
        }

        public override void OnEnter(bool _excludeAction = false)
        {
            patrolSettings.patrolAreaIndex = Random.Range(0, patrolSettings.patrolAreas.Count);
            nodes = new Queue<PatrolNode>(patrolSettings.patrolAreas[patrolSettings.patrolAreaIndex].nodes);
            nodesLeft = Random.Range(patrolSettings.nodesBeforeRestRange.min, patrolSettings.nodesBeforeRestRange.max);
            SetRandomTarget();
            targetDestination = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        }

        public override void UpdateState(IBrain _brain)
        {
            if (lookAtTarget)
            {
                agentController.transform.LookAt(lookAtTarget);
            }

            if (nodesLeft <= 0)
            {
                patrolSettings.actionAfterPatrolArea?.Invoke();
                UpdatePatrolArea();
                agentController.mainBrain.TransitionToNext();
            }

            if (!target)
            {
                return;
            }

            if (agentController.pathfinding.GetDestination().SqrDistance(targetDestination, true) > 0.5f)
            {
                agentController.pathfinding.SetDestination(targetDestination);
            }

            if (!runningRoutine && agentController.transform.WithinRange(targetDestination, patrolSettings.maxNodeDistance))
            {
                agentController.StartCoroutine(PerformNodeAction());
            }
        }

        public override void OnExit()
        {
            agentController.pathfinding.SetDestination(agentController.transform.position);
        }

        /// <summary>
        /// Go to the next patrol area.
        /// </summary>
        private void UpdatePatrolArea()
        {
            patrolSettings.patrolAreaIndex++;

            if (patrolSettings.patrolAreaIndex >= patrolSettings.patrolAreas.Count)
            {
                patrolSettings.patrolAreaIndex = 0;
            }

            nodes = new Queue<PatrolNode>(patrolSettings.patrolAreas[patrolSettings.patrolAreaIndex].nodes);
            SetRandomTarget();

            nodesLeft = Random.Range(patrolSettings.nodesBeforeRestRange.min, patrolSettings.nodesBeforeRestRange.max);
        }

        /// <summary>
        /// Find a random node and use it as target.
        /// </summary>
        private void SetRandomTarget()
        {
            int range = Random.Range(1, nodes.Count);
            for (int i = 0; i < range; i++)
            {
                if (target)
                {
                    nodes.Enqueue(target);
                }

                target = nodes.Dequeue();
            }
        }

        /// <summary>
        /// Perform the action that the node tells to perform.
        /// </summary>
        private IEnumerator PerformNodeAction()
        {
            runningRoutine = true;

            int waitTime = 0;

            Node node = NodeManager.Instance.GetNodeById(target.node.id);
            if (node.animationType != AnimationType.NONE)
            {
                waitTime = Random.Range(node.interactTime.x, node.interactTime.y);

                if (node.interactObject != null)
                {
                    lookAtTarget = node.interactObject;
                }

                Debug.Log(agentController.gameObject.name + " is performing " + node.animationType, agentController.gameObject);
                // TIP: An animation can be fired at this position instead of the debug.
                //agentController.AnimationHandler.SetTrigger(node.interactOption.ToString());
            }

            yield return new WaitForSeconds(waitTime);

            if (patrolSettings.randomNodes)
            {
                SetRandomTarget();
            }
            else
            {
                lookAtTarget = null;
                nodes.Enqueue(target);
                target = nodes.Dequeue();
            }

            float nodeSpread;
            Vector3 nodePosition;
            nodePosition = target.transform.position;
            nodeSpread = Random.Range(target.node.nodeSpread.x, target.node.nodeSpread.y) / 2f;

            if (priorityTarget != null)
            {
                prevPrioritiyTarget = priorityTarget;
            }

            // Select a new priority node, if it was the same as before don't and continue with the normal target
            if (Random.Range(0f, 1f) > patrolSettings.prioritySensitivity)
            {
                List<PatrolNode> prioNodes = nodes.ToList();
                int val = 0;
                List<PatrolNode> options = new();
                foreach (PatrolNode prioNode in prioNodes)
                {
                    if (prioNode.node.priority > 0)
                    {
                        val += prioNode.node.priority;
                        for (int i = 0; i < prioNode.node.priority; i++)
                        {
                            options.Add(prioNode);
                        }
                    }
                }
                priorityTarget = options[Random.Range(0, options.Count)];

                if (prevPrioritiyTarget == priorityTarget)
                {
                    prevPrioritiyTarget = null;
                    priorityTarget = null;
                }
                else
                {
                    nodePosition = priorityTarget.transform.position;
                    nodeSpread = Random.Range(priorityTarget.node.nodeSpread.x, priorityTarget.node.nodeSpread.y) / 2f;
                }
            }

            targetDestination = new Vector3(
                    Random.Range(nodePosition.x - nodeSpread, nodePosition.x + nodeSpread),
                    nodePosition.y,
                    Random.Range(nodePosition.z - nodeSpread, nodePosition.z + nodeSpread));
            nodesLeft--;

            patrolSettings.actionAfterPatrolNode?.Invoke();
            yield return null;
            runningRoutine = false;
        }
    }
}
