using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// This interface can be used as means to implement a different pathfinding solution.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Set a destination to go to.
        /// </summary>
        /// <param name="_destination"></param>
        public void SetDestination(Vector3 _destination);

        /// <summary>
        /// Retrieve the current destination.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetDestination();

        /// <summary>
        /// Set the speed.
        /// </summary>
        /// <param name="_speed"></param>
        public void SetSpeed(float _speed);
    }
}
