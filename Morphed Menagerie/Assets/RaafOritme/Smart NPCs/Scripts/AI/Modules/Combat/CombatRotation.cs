using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [CreateAssetMenu(fileName = "Combat Rotation", menuName = "RaafOritme/Smart NPCs/Create Combat Rotation", order = 2)]
    [System.Serializable]
    public class CombatRotation : ScriptableObject
    {
        public int meleeDamage = 5;
        public int rangeDamage = 3;
        public List<CombatContainer> combatRotations;
    }

    [System.Serializable]
    public class CombatContainer
    {
        // This is the only way I could get it serialized
        [SerializeReference, SubclassPicker]
        public BaseRotation combat;
    }
}
