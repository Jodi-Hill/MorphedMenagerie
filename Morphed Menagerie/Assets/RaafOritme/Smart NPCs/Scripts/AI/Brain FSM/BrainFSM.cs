using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// This is a simple lightweight brain for routine-based behaviour.
    /// </summary>
    public class BrainFSM : AbstractBrain
    {
        public BaseModule currentState;
        public readonly Queue<BaseModule> stateRoutine = new();

        private GameObject gameObject;

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
            AllModules = new List<BaseModule>();
            lastActions = new();

            foreach (Container container in _routine.routine)
            {
                BaseModule module = CreateInstance(container.module.GetType());
                AllModules.Add(module);
                module.Initialize(agentController);
            }
        }

        public override void Start()
        {
            if (AllModules.Count > 0)
            {
                foreach (BaseModule state in AllModules)
                {
                    stateRoutine.Enqueue(state);
                }
            }
            else
            {
                Debug.LogError("States have not been added!", gameObject);
            }

            currentState = stateRoutine.Dequeue();
            currentState.OnEnter();
        }

        public override void Update()
        {
            currentState?.UpdateState(this);
            currentAction = currentState?.GetType().Name;
        }

        /// <summary>
        /// TransitionToAction to the next state in the routine.
        /// </summary>
        public override void TransitionToNext()
        {
            lastActions.Add(currentState.GetType().Name);
            stateRoutine.Enqueue(currentState);
            currentState?.OnExit();
            currentState = stateRoutine.Dequeue();
            currentState.OnEnter();
            agentController.AnimationHandler.BackToDefault();
        }

        /// <summary>
        /// Transition to a specific state due to an external factor.
        /// </summary>
        /// <param name="_state"></param>
        public override void TransitionToState(OverRuleState _state)
        {
            // This can be made more dynamic by using reflection
            agentController.AnimationHandler.BackToDefault();
            switch (_state)
            {
                default:
                    currentState?.OnEnter();
                    break;

                case OverRuleState.COMBAT:
                    ExecuteModule(typeof(CombatModule));
                    break;

                case OverRuleState.CHATTING:
                case OverRuleState.DECEASED:
                    currentState?.OnExit();
                    break;
            }
        }

        /// <summary>
        /// Finds and executes specific module.
        /// </summary>
        /// <param name="_type"></param>
        private void ExecuteModule(Type _type)
        {
            foreach (BaseModule module in AllModules)
            {
                if (module.GetType() == _type)
                {
                    for (int i = 0; i < AllModules.Count; i++)
                    {
                        TransitionToNext();
                        if (currentState.GetType() == _type)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
