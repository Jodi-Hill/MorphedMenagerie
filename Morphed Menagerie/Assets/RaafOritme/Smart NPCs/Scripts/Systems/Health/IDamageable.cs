using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public interface IDamageable
    {
        /// <summary>
        /// Entity receives damage and could potentially do something with the sender.
        /// </summary>
        /// <param name="_damage"></param>
        /// <param name="_sender"></param>
        public void TakeDamage(int _damage, Transform _sender);
    }
}
