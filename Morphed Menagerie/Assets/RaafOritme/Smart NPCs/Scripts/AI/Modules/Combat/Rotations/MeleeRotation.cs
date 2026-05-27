using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class MeleeRotation : BaseRotation
    {
        public override void Initialize(CombatModule _agentModule)
        {
            agentModule = _agentModule;
        }

        public override void Invoke()
        {
            Debug.Log("Melee damage: " + agentModule.currentRotation.meleeDamage);
        }
    }
}
