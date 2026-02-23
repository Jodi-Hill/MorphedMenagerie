using UnityEngine;
using UnityEngine.AI;

namespace RaafOritme.SmartNPCs
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Pathfinding : MonoBehaviour, INavigation
    {
        private NavMeshAgent navmeshAgent;

        private void Awake()
        {
            navmeshAgent = GetComponent<NavMeshAgent>();
        }

        public void SetDestination(Vector3 _destination)
        {
            if (!navmeshAgent.enabled)
            {
                return;
            }

            navmeshAgent.destination = _destination;
        }

        public Vector3 GetDestination()
        {
            return navmeshAgent.destination;
        }

        public void SetSpeed (float _speed)
        {
            navmeshAgent.speed = _speed;
        }

        /// <summary>
        /// Is this component capable of navigating?
        /// </summary>
        /// <returns></returns>
        public bool CanNavigate()
        {
            return navmeshAgent.enabled;
        }
    }
}
