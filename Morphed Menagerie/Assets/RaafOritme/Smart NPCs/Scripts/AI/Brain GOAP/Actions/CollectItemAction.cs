using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class CollectItemAction : BaseAction
    {
        public CollectItemAction()
        {
            actionName = "Collect Item";
            actionCost = 2.0f;
        }

        public override void OnEnter(BrainGOAP _npc)
        {
        }

        public override void Execute(BrainGOAP _npc)
        {
            Inventory inventory = _npc.inventory;

            if (hasRequirement && inventory.HasRequirement(requirement, requiredAmount))
            {
                if (hasRequirement && inventory.HasRequirement(requirement, requiredAmount))
                {
                    inventory.RemoveFromInventory(requirement, requiredAmount);
                }
                else
                {
                    Debug.Log("Missing right amount of item!");
                    return;
                }
            }

            // TIP: Tell brain to go to place where item can be collected, through for example a callback or by using the navigation stack from the brain.

            if (result != ObjectDatabase.Empty)
            {
                inventory.AddToInventory(result, givenAmount);
            }
        }
    }
}
