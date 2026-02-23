using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class ActionPlanner
    {
        public float scanRadius = 50;

        /// <summary>
        /// Chooses which action is the best to perform.
        /// </summary>
        /// <param name="_npc"></param>
        /// <param name="_actionUtilities"></param>
        /// <returns></returns>
        public BaseAction SelectBestAction(BrainGOAP _npc, Dictionary<BaseAction, float> _actionUtilities)
        {
            BaseAction bestAction = null;
            float bestScore = float.MaxValue;

            foreach (KeyValuePair<BaseAction, float> actionUtility in _actionUtilities)
            {
                if (actionUtility.Key.CanExecute(_npc))
                {
                    float actionCost = actionUtility.Key.GetActionCost(_npc) + actionUtility.Value;
                    if (actionCost < bestScore)
                    {
                        bestScore = actionCost;
                        bestAction = actionUtility.Key;
                    }
                }
            }

            return bestAction;
        }

        /// <summary>
        /// Returns a list of all actions within a range to perform.
        /// </summary>
        /// <param name="_npc"></param>
        /// <returns></returns>
        public List<BaseAction> ScanForActions(BrainGOAP _npc)
        {
            return ActionManager.Instance.GetNearbyActions(_npc.gameObject.transform, scanRadius, _npc);
        }

        /// <summary>
        /// This section of the code is used as an example for how you can calculate nested goals in order to reach an end goal.
        /// This logic follows A* pathfinding but used for actions so that you can have a queue of actions.
        /// The NPC will have to finish the queue in order to reach a specific goal and once that has been reached it will find a new path for a new goal.
        /// </summary>
       
        // TIP: An example about how you can create nested actions where the GOAP receives a set of actions to perform before completing its objective.
        #region GOAP nested
        public BaseAction endGoal;
        public List<BaseAction> actionsToGoal;
        public List<Vector3> pathToActions = new List<Vector3>();

        public void SelectRandomGoal(BrainGOAP _npc)
        {
            List<BaseAction> availableActions = ScanForActions(_npc);
            if (actionsToGoal.Count == 0)
            {
                endGoal = availableActions[Random.Range(0, availableActions.Count)];
                pathToActions = FindPathToTarget(endGoal, _npc);
            }
        }

        public void SelectGoal(BaseAction _goal, BrainGOAP _npc)
        {
            endGoal = _goal;
            pathToActions = FindPathToTarget(endGoal, _npc);
        }

        public void CompleteStep(BrainGOAP _npc)
        {
            if (actionsToGoal.Count > 0)
            {
                actionsToGoal[0].Execute(_npc);
            }

            pathToActions.RemoveAt(0);
            actionsToGoal.RemoveAt(0);
        }

        public List<Vector3> FindPathToTarget(BaseAction _goal, BrainGOAP _npc)
        {
            List<BaseAction> openSet = new List<BaseAction>();
            HashSet<BaseAction> closedSet = new HashSet<BaseAction>();
            openSet.Add(_goal);
            _goal.quantityStack = _goal.requiredAmount;

            while (openSet.Count > 0)
            {
                BaseAction currentAction = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (currentAction.hasRequirement && openSet[i].result == currentAction.requirement)
                    {
                        currentAction = openSet[i];
                    }
                }

                openSet.Remove(currentAction);
                closedSet.Add(currentAction);

                // if the actionUtility has no requirement or if the enemy has a sufficient amount of the item return a route
                if (!currentAction.hasRequirement || _npc.inventory.HasRequirement(currentAction.requirement, currentAction.requiredAmount))
                {
                    List<Vector3> path = new List<Vector3>();
                    List<BaseAction> actionPath = RetracePath(_goal, _npc);
                    for (int i = 0; i < actionPath.Count; i++)
                    {
                        path.Add(actionPath[i].transform.position);
                    }
                    return path;
                }

                foreach (BaseAction checkable in GetActions(currentAction, _npc))
                {
                    // skip this iteration if actionUtility is unreachable or if we already checked it
                    if (closedSet.Contains(checkable))
                    {
                        continue;
                    }

                    float distanceCost = Mathf.RoundToInt(Vector3.Distance(checkable.transform.position, currentAction.transform.position));
                    float reqCost = RequirementCost(currentAction, checkable, _npc);
                    float costToAction = currentAction.FScore + reqCost;
                    if (costToAction < checkable.FScore || !openSet.Contains(checkable))
                    {
                        currentAction.parent = checkable;
                        checkable.child = currentAction;
                        checkable.quantityStack = currentAction.quantityStack * checkable.requiredAmount;
                        checkable.GScore = distanceCost;
                        checkable.HScore = costToAction;
                        checkable.quantity = CalculateAmountNeeded(currentAction, checkable, _npc);

                        if (!openSet.Contains(checkable))
                        {
                            openSet.Add(checkable);
                        }
                    }
                }
            }

            Debug.Log("Couldnt find a path!");
            return null;
        }

        // Calculate costs based on actions and quantity
        public float RequirementCost(BaseAction _requirement, BaseAction _giver, BrainGOAP _npc)
        {
            float cost = 0;

            if (_requirement.hasRequirement)
            {
                cost = CalculateAmountNeeded(_requirement, _giver, _npc) * _giver.actionCost;
            }

            return cost;
        }

        // Calculate how many items are needed
        public int CalculateAmountNeeded(BaseAction _requirement, BaseAction _giver, BrainGOAP _npc)
        {
            int amount = 0;
            amount = Mathf.CeilToInt((_requirement.requiredAmount * _requirement.quantityStack - _npc.inventory.HasAmountOfItem(_requirement.requirement)) / _giver.givenAmount);
            return amount;
        }

        // Retrace path and make sure that other lists get updated as well
        private List<BaseAction> RetracePath(BaseAction _goal, AbstractBrain _npc)
        {
            List<BaseAction> path = new List<BaseAction>();
            BaseAction currentAction = _goal;
            BaseAction prevAction = _goal;
            actionsToGoal = new List<BaseAction>();

            while (currentAction != null)
            {
                if (!path.Contains(currentAction))
                {
                    int amount = (currentAction.child != null) ? currentAction.child.quantityStack : 0;

                    if (prevAction != currentAction)
                    {
                        for (int i = 0; i < (amount - _npc.inventory.HasAmountOfItem(currentAction.result)) / currentAction.givenAmount; i++)
                        {
                            path.Add(currentAction);
                            actionsToGoal.Add(currentAction);
                        }
                    }
                    else
                    {
                        path.Add(currentAction);
                        actionsToGoal.Add(currentAction);
                    }

                    if (currentAction.parent != null)
                    {
                        prevAction = currentAction;
                        currentAction = currentAction.parent;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Debug.Log("Failed to calculate whole path, triggered infinite loop");
                    break;
                }
            }

            actionsToGoal.Reverse();
            path.Reverse();
            return path;
        }

        public List<BaseAction> GetActions(BaseAction _action, BrainGOAP _npc)
        {
            List<BaseAction> options = new List<BaseAction>();
            List<BaseAction> availableActions = ScanForActions(_npc);

            for (int i = 0; i < availableActions.Count; i++)
            {
                if (availableActions[i].result == _action.requirement)
                {
                    BaseAction option = availableActions[i];
                    options.Add(option);
                }
            }

            return options;
        }
        #endregion
    }
}
