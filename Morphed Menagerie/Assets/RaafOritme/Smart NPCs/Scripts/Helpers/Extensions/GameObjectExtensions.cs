using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// This extension returns component T.
        /// </summary>
        public static T TryAddComponent<T>(this GameObject _origin) where T : Component
        {
            T component = _origin.GetComponent<T>();

            if (!component)
            {
                component = _origin.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// Get a component if <paramref name="_destination"/> has not been set yet.
        /// Otherwise, just return <paramref name="_destination"/>.
        /// </summary>
        /// <param name="_origin">The GameObject to get the component from.</param>
        /// <param name="_destination">The variable to cache the component into.</param>
        public static T GetAndCacheComponent<T>(this GameObject _origin, ref T _destination) where T : Component
        {
            if (!_destination)
            {
                _destination = _origin.GetComponent<T>();
            }

            return _destination;
        }
    }
}
