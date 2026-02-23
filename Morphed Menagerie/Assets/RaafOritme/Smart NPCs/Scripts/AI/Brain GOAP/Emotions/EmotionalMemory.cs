using System.Collections.Generic;

namespace RaafOritme.SmartNPCs
{
    public class EmotionalMemory
    {
        // action -> success/failure count
        private Dictionary<string, int> pastDecisions = new Dictionary<string, int>();

        /// <summary>
        /// Track outcomes of decisions in success/failure count.
        /// </summary>
        /// <param name="_action"></param>
        /// <param name="_success"></param>
        public void RecordDecision(string _action, bool _success)
        {
            _action = _action.ToLower();

            if (pastDecisions.ContainsKey(_action))
                pastDecisions[_action] += _success ? 1 : -1;
            else
                pastDecisions.Add(_action, _success ? 1 : -1);
        }

        /// <summary>
        /// Usable as influence for decision making.
        /// </summary>
        /// <param name="_action"></param>
        /// <returns></returns>
        public float GetMemoryInfluence(string _action)
        {
            _action = _action.ToLower();

            if (pastDecisions.ContainsKey(_action))
            {
                return pastDecisions[_action] > 0 ? 1.2f : 0.8f; // bias towards positive experiences
            }
            return 1.0f; // neutral influence if there is no memory
        }
    }
}
