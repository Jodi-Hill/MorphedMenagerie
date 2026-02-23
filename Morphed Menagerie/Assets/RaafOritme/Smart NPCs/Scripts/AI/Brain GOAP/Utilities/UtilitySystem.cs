using System;
using System.Collections.Generic;

namespace RaafOritme.SmartNPCs
{
    public class UtilitySystem
    {
        private List<IUtilityFactor> factors;
        private BrainGOAP brainGoap;

        public UtilitySystem(BrainGOAP _brainGoap) 
        {
            brainGoap = _brainGoap;
            factors = new();
        }

        /// <summary>
        /// Adds a specific factor.
        /// </summary>
        /// <param name="_factor"></param>
        public void AddFactor(IUtilityFactor _factor)
        {
            factors.Add(_factor);
        }

        /// <summary>
        /// Return the weight of all actions. Essentially, how likely will the NPC be to perform a certain action.
        /// </summary>
        /// <param name="_actions"></param>
        /// <returns></returns>
        public Dictionary<BaseAction, float> CalculateUtilities(List<BaseAction> _actions)
        {
            Dictionary<BaseAction, float> actionWeights = new Dictionary<BaseAction, float>();

            foreach (BaseAction action in _actions)
            {
                actionWeights.Add(action, CalculateActionUtility(action));
            }

            return actionWeights;
        }

        /// <summary>
        /// Calculate the utility for an action.
        /// </summary>
        /// <param name="_action"></param>
        /// <returns></returns>
        public float CalculateActionUtility(BaseAction _action)
        {
            float totalUtility = _action.priority;
            foreach (var factor in factors)
            {
                totalUtility += factor.CalculateUtility(brainGoap, _action);
            }
            return totalUtility;
        }

        /// <summary>
        /// Select the best action to perform.
        /// </summary>
        /// <param name="_availableActions"></param>
        /// <returns></returns>
        public BaseAction SelectBestAction(List<BaseAction> _availableActions)
        {
            BaseAction bestAction = null;
            float highestUtility = float.MinValue;

            foreach (BaseAction action in _availableActions)
            {
                float utility = CalculateActionUtility(action);
                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            return bestAction;
        }
    }
}
