using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [CreateAssetMenu(fileName = "AI Routine", menuName = "RaafOritme/Smart NPCs/Create AI routine", order = 1)]
    [System.Serializable]
    public class AIRoutine : ScriptableObject
    {
        public List<Container> routine;
    }

    [System.Serializable]
    public class Container
    {
        // This is the only way I could get it serialized
        [SerializeReference, SubclassPicker]
        public BaseModule module;
    }
}
