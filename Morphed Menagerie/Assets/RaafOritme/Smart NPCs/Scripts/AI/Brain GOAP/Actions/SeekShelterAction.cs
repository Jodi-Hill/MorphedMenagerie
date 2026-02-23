using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class SeekShelterAction : BaseAction
    {
        public SeekShelterAction()
        {
            actionName = "Seek Shelter";
            actionCost = 5.0f;
            preconditions.Add(Conditions.UnderAttack, true);
            effects.Add(Effects.Safe, true);
        }

        public override void OnEnter(BrainGOAP _npc)
        {
        }

        public override void Execute(BrainGOAP _npc)
        {
            // TIP: NPC retreat behaviour; go to residence with pathfinding module or a nearby safe space.
            Debug.Log(_npc.gameObject.name + " is seeking shelter!");
        }
    }
}
