using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public abstract class BaseEnvironment : MonoBehaviour
    {
        /// <summary>
        /// Ensure that the scene is GOAP ready by setting all basics.
        /// </summary>
        public abstract void Goapify();
    }
}
