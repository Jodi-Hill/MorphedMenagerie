using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class ActionManager : MonoBehaviour
    {
        private static ActionManager instance;
        public static ActionManager Instance => instance;

        private List<BaseAction> actions = new();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Cache action in a quick lookup table.
        /// </summary>
        /// <param name="_action"></param>
        public void RegisterAction(BaseAction _action)
        {
            if (!actions.Contains(_action))
            {
                actions.Add(_action);
            }
        }

        /// <summary>
        /// Scans for all actions and returns a list of those within range.
        /// </summary>
        /// <param name="_agentTransform"></param>
        /// <param name="_radius"></param>
        /// <param name="_npcBrain"></param>
        /// <returns></returns>
        public List<BaseAction> GetNearbyActions(Transform _agentTransform, float _radius, BrainGOAP _npcBrain)
        {
            return actions
                .Where(action => _agentTransform.WithinRange(action.transform.position, _radius))
                .Where(action => action.CanExecute(_npcBrain))
                .ToList();
        }
    }
}
