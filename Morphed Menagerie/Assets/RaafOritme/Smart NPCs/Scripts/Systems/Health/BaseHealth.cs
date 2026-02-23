using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public abstract class BaseHealth : MonoBehaviour, IDamageable
    {
        private int health;

        public virtual void TakeDamage(int _damage, Transform _sender)
        {
            health -= _damage;

            if (health < 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Entity dies.
        /// </summary>
        public abstract void Die();
    }
}
