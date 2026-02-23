using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RaafOritme.SmartNPCs
{
    public abstract class BaseAction : MonoBehaviour
    {
        public string actionName; // used for NPC memories
        public float actionCost = 1.0f;
        public float priority = 1.0f;
        public Modules requiredModule;
        public Dictionary<Conditions, bool> preconditions = new Dictionary<Conditions, bool>();
        public Dictionary<Effects, bool> effects = new Dictionary<Effects, bool>();

        public ObjectDatabase result;
        public int givenAmount = 0;
        public bool hasRequirement;
        public ObjectDatabase requirement;
        public int requiredAmount = 0;
        public UnityEvent onActionCompleted;

        /// <summary>
        /// Executes a specific task when all criteria has been met.
        /// </summary>
        /// <param name="_npc"></param>
        public abstract void Execute(BrainGOAP _npc);

        /// <summary>
        /// NPC is going to execute this task, so the NPC should be prepared.
        /// </summary>
        /// <param name="_npc"></param>
        public abstract void OnEnter(BrainGOAP _npc);

        protected void Start()
        {
            ActionManager.Instance.RegisterAction(this);
        }

        /// <summary>
        /// Check if all conditions are met in order to execute this action.
        /// </summary>
        /// <param name="_npc"></param>
        /// <returns></returns>
        public virtual bool CanExecute(BrainGOAP _npc)
        {
            if (!_npc.agentController.modules.HasFlag(requiredModule))
            {
                return false;
            }

            foreach (KeyValuePair<Conditions, bool> keyValue in preconditions)
            {
                if (_npc.GetCondition(keyValue.Key) != keyValue.Value)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Upon completion give the npc all effects as a reward.
        /// </summary>
        /// <param name="_npc"></param>
        public virtual void OnComplete(BrainGOAP _npc)
        {
            foreach (KeyValuePair<Effects, bool> keyValue in effects)
            {
                _npc.UpdateEffect(keyValue.Key, keyValue.Value);
            }
            onActionCompleted?.Invoke();
            _npc.ReceiveReward(result, givenAmount);
            _npc.decisionSystem.emotionSystem.emotionalMemory.RecordDecision(actionName, true);
        }

        /// <summary>
        /// NPC has somehow failed to complete the action.
        /// </summary>
        /// <param name="_npc"></param>
        public virtual void OnFailure(BrainGOAP _npc)
        {
            _npc.decisionSystem.emotionSystem.emotionalMemory.RecordDecision(actionName, false);
        }

        /// <summary>
        /// Calculate how much this action costs to execute with an emotional bias.
        /// </summary>
        /// <param name="_npc"></param>
        /// <returns></returns>
        public float GetActionCost(BrainGOAP _npc)
        {
            EmotionType emotionType = _npc.decisionSystem.emotionSystem.emotionalState.GetLeadingEmotion();
            float emotionModifier = _npc.decisionSystem.emotionSystem.emotionalState.GetEmotionValue(emotionType);
            float personalityModifier = _npc.decisionSystem.emotionSystem.personality.ModifyBasedOnPersonality(emotionType, emotionModifier);
            float memoryModifier = _npc.decisionSystem.emotionSystem.emotionalMemory.GetMemoryInfluence(actionName);

            return actionCost * personalityModifier * memoryModifier;
        }

        // TIP: can be used for the A-STAR in the planner that is nested
        #region A* fields
        public float FScore
        {
            get { return GScore + HScore; }
        }
        [HideInInspector]
        public float GScore;
        [HideInInspector]
        public float HScore;
        [HideInInspector]
        public BaseAction parent;
        [HideInInspector]
        public BaseAction child;
        [HideInInspector]
        public int quantity = 1;
        [HideInInspector]
        public int quantityStack; // stores total amount of quantity during this calculation in order to calculate actual quantity
        #endregion
    }
}
