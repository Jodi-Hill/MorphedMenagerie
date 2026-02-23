using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public abstract class AbstractBrain : IBrain
    {
        public Inventory inventory;
        public AgentController agentController { get; set; }
        public List<BaseModule> AllModules { get; set; } = new();

        public string currentAction { get; set; }
        public List<string> lastActions { get; set; }

        public abstract void Initialize(AIRoutine _routine, GameObject _gameObject, AgentController _controller);

        public abstract void Start();

        public abstract void Update();

        public abstract void TransitionToNext();
        public abstract void TransitionToState(OverRuleState _state);

        /// <summary>
        /// This method ensures that any module type received as a type is created and instantiated. 
        /// This method allows you to convert an enum to a class, or more specifically, a module.
        /// </summary>
        /// <param name="_moduleType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected BaseModule CreateInstance(Type _moduleType)
        {
            // Ensure the type is a subclass of BaseModule and has a parameterless constructor to prevent parameter issues
            if (_moduleType.IsSubclassOf(typeof(BaseModule)) && _moduleType.GetConstructor(Type.EmptyTypes) != null)
            {
                return (BaseModule)Activator.CreateInstance(_moduleType);
            }
            else
            {
                throw new InvalidOperationException($"Type {_moduleType} cannot be instantiated.");
            }
        }
    }
}
