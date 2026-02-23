using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public interface IBrain
    {
        public string currentAction { get; set; }
        public List<string> lastActions { get; set; }

        public AgentController agentController { get; set; }

        /// <summary>
        /// Remove inactive modules and add active modules.
        /// </summary>
        /// <param name="_routine"></param>
        /// <param name="_gameObject"></param>
        /// <param name="_controller"></param>
        public abstract void Initialize(AIRoutine _routine, GameObject _gameObject, AgentController _controller);

        /// <summary>
        /// The brains arent monobehaviour so they require a manual start.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Required to update the base routine.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Go to the next state.
        /// </summary>
        public abstract void TransitionToNext();

        /// <summary>
        /// Transition to a specific state.
        /// </summary>
        /// <param name="_state"></param>
        public abstract void TransitionToState(OverRuleState _state);
    }
}
