using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns a lightweight squared squaredDistance between 2 vector3s.
        /// </summary>
        public static float SqrDistance(this Vector3 _positionA, Vector3 _positionB)
        {
            return (_positionA - _positionB).sqrMagnitude;
        }

        /// <summary>
        /// Returns a lightweight squared squaredDistance between 2 vector3s while ignoring the difference on the Y axis.
        /// </summary>
        public static float SqrDistance(this Vector3 _positionA, Vector3 _positionB, bool _ignoreY)
        {
            _positionA = new Vector3(_positionA.x, 0, _positionA.z);
            _positionB = new Vector3(_positionB.x, 0, _positionB.z);
            return (_positionA - _positionB).sqrMagnitude;
        }
    }
}
