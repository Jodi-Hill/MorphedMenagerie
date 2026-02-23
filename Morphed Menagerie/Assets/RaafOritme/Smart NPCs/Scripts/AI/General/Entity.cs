using System;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class Entity : BaseHealth
    {
        [Header("Personalization")]
        public Vitality vitality;
        public Vitality strength;

        private AgentController agent;
        private AgentController Agent => gameObject.GetAndCacheComponent(ref agent);

        public bool dead = false;

        public Animator Animator { get; set; }
        
        public Action<Transform> damageCallback;

        /// <summary>
        /// Set the statistics for the entity.
        /// </summary>
        /// <param name="_vitality"></param>
        /// <param name="_strength"></param>
        public void SetStatistics(Vitality _vitality, Vitality _strength)
        {
            vitality = _vitality;
            strength = _strength;
        }
        
        public override void TakeDamage(int _damage, Transform _sender)
        {
            if (dead)
            {
                return;
            }

            base.TakeDamage(_damage, _sender);
            damageCallback?.Invoke(_sender);
        }

        public override void Die()
        {
            if (dead)
            {
                return;
            }

            dead = true;

            if (Agent)
            {
                Agent.SetOverrule(OverRuleState.DECEASED);
            }
        }
    }
}
