using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// This is the heavy weight brain that encapsulates more complexity for dynamic behaviour.
    /// </summary>
    public class BrainGOAP : AbstractBrain
    {
        public GameObject gameObject;
        public DecisionSystem decisionSystem;
        public Personality personality;
        public PersonalityType personalityType;

        public Dictionary<Conditions, bool> conditions = new Dictionary<Conditions, bool>();
        public Dictionary<Effects, bool> effects = new Dictionary<Effects, bool>();

        private BaseAction activeAction;

        /// <summary>
        /// Adds modules based on settings through reflection.
        /// </summary>
        /// <param name="_routine"></param>
        /// <param name="_gameObject"></param>
        /// <param name="_controller"></param>
        public override void Initialize(AIRoutine _routine, GameObject _gameObject, AgentController _controller)
        {
            gameObject = _gameObject;
            agentController = _controller;
            AllModules = new();
            lastActions = new();
            personality = new(personalityType);
            decisionSystem = new(this);
            inventory = new();

            foreach (Container container in _routine.routine)
            {
                BaseModule module = CreateInstance(container.module.GetType());
                if (!AllModules.Contains(module))
                {
                    AllModules.Add(module);
                    module.Initialize(agentController);
                }
            }

            TransitionToNext();
        }

        public override void Start()
        {
        }

        private void UpdateAction(BaseAction _action)
        {
            if (_action == null)
            {
                // What should happen when the GOAP can't find an action?
                Debug.LogError("GOAP brain couldn't find action to perform.", gameObject);
                return;
            }

            activeAction = _action;
            lastActions.Add(activeAction.GetType().Name);
            currentAction = activeAction.GetType().Name;
            activeAction.OnEnter(this);
        }

        public override void TransitionToNext()
        {
            if (activeAction != null)
            {
                activeAction.OnComplete(this);
            }

            UpdateAction(decisionSystem.MakeDecision());
        }

        public override void TransitionToState(OverRuleState _state)
        {
            // TIP: Can use this method to transition to a specific state if desired, for example immediate combat when getting attacked.
        }

        // TIP: This method can be optimized by implementing delays or by using for example LateUpdate, if desired so.
        public override void Update()
        {
            decisionSystem?.UpdateSystems();
            activeAction?.Execute(this);
        }

        /// <summary>
        /// Check if NPC has a specific condition.
        /// </summary>
        /// <param name="_condition"></param>
        /// <returns></returns>
        public bool GetCondition(Conditions _condition)
        {
            if (conditions.ContainsKey(_condition))
            {
                return conditions[_condition];
            }
            return false;
        }

        /// <summary>
        /// Check if NPC has a specific effect.
        /// </summary>
        /// <param name="_effect"></param>
        /// <returns></returns>
        public bool GetEffect(Effects _effect)
        {
            if (effects.ContainsKey(_effect))
            {
                return effects[_effect];
            }
            return false;
        }

        /// <summary>
        /// Update a condition.
        /// </summary>
        /// <param name="_condition"></param>
        /// <param name="_value"></param>
        public void UpdateCondition(Conditions _condition, bool _value)
        {
            if (conditions.ContainsKey(_condition))
            {
                conditions[_condition] = _value;
            }
            else
            {
                conditions.Add(_condition, _value);
            }

            switch (_condition)
            {
                case Conditions.UnderAttack:
                    decisionSystem.emotionSystem.emotionalState.UpdateType(EmotionType.Fear, 50);
                    break;
            }
        }

        /// <summary>
        /// Update an effect.
        /// </summary>
        /// <param name="_effect"></param>
        /// <param name="_value"></param>
        public void UpdateEffect(Effects _effect, bool _value)
        {
            if (effects.ContainsKey(_effect))
            {
                effects[_effect] = _value;
            }
            else
            {
                effects.Add(_effect, _value);
            }

            switch (_effect)
            {
                case Effects.Safe:
                    decisionSystem.emotionSystem.emotionalState.UpdateType(EmotionType.Fear, -20);
                    break;
            }
        }

        /// <summary>
        /// Award the NPC for completing something.
        /// </summary>
        /// <param name="_reward"></param>
        /// <param name="_amount"></param>
        public void ReceiveReward(ObjectDatabase _reward, int _amount)
        {
            switch (_reward)
            {
                case ObjectDatabase.Empty:
                    break;
                case ObjectDatabase.Stamina:
                    decisionSystem.utilities.fatigue -= _amount;
                    break;
                case ObjectDatabase.Fatigue:
                    decisionSystem.utilities.fatigue += _amount;
                    break;
                case ObjectDatabase.Food:
                    decisionSystem.utilities.hunger -= _amount;
                    break;
                case ObjectDatabase.Coins:
                    break;
            }
        }
    }
}
