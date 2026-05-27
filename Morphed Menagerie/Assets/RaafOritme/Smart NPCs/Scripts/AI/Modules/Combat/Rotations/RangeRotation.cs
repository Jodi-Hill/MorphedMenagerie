using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class RangeRotation : BaseRotation
    {
        public override void Initialize(CombatModule _agentModule)
        {
            agentModule = _agentModule;
        }

        public override void Invoke()
        {
            Debug.Log("Range damage: " + agentModule.currentRotation.rangeDamage);
        }
    }
}
