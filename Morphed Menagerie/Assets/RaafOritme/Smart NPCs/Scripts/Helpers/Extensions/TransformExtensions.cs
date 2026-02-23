using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Returns true if within range.
        /// </summary>
        public static bool WithinRange(this Transform _transform, Vector3 _target, float _range)
        {
            float sqrRange = _range * _range;
            return (_transform.position.SqrDistance(_target) < sqrRange);
        }

        /// <summary>
        /// Returns the size of colliders in range and requires a colliders array as reference.
        /// </summary>
        public static int CollidersInRange(this Transform _transform, float _scanRadius, ref Collider[] _colliders, LayerMask _layerMask)
        {
            return Physics.OverlapSphereNonAlloc(_transform.position, _scanRadius, _colliders, _layerMask, QueryTriggerInteraction.Collide);
        }

        /// <summary>
        /// Returns the squaredDistance between 2 transforms squared, this is a lightweight squaredDistance calculator.
        /// </summary>
        /// <returns></returns>
        public static float SquaredDistance(this Transform _transform, Transform _target)
        {
            return _transform.position.SqrDistance(_target.position);
        }

        /// <summary>
        /// Returns the squaredDistance between 2 transforms squared, this is a lightweight squaredDistance calculator.
        /// </summary>
        /// <returns></returns>
        public static float SquaredDistance(this Transform _transform, Vector3 _target)
        {
            return _transform.position.SqrDistance(_target);
        }
    }
}
