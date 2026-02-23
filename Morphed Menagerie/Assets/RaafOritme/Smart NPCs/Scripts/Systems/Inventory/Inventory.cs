using System;
using System.Linq;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [Serializable]
    public class Inventory
    {
        [SerializeField]
        public ItemDictionary ItemCollection = new ItemDictionary();

        /// <summary>
        /// Check if item exists in inventory.
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public bool HasItem(ObjectDatabase _item)
        {
            return ItemCollection.ToDictionary().ContainsKey(_item);
        }

        /// <summary>
        /// Return amount of item from inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int HasAmountOfItem(ObjectDatabase item)
        {
            if (ItemCollection.ToDictionary().ContainsKey(item))
            {
                return ItemCollection.ToDictionary()[item];
            }

            return 0;
        }

        /// <summary>
        /// Check if conditions are met.
        /// </summary>
        /// <param name="_requiredItem"></param>
        /// <param name="_requiredAmount"></param>
        /// <returns></returns>
        public bool HasRequirement(ObjectDatabase _requiredItem, int _requiredAmount)
        {
            return (ItemCollection.ToDictionary().ContainsKey(_requiredItem) && ItemCollection.ToDictionary()[_requiredItem] >= _requiredAmount);
        }

        /// <summary>
        /// Add an x amount of item to the inventory.
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_amount"></param>
        public void AddToInventory(ObjectDatabase _item, int _amount)
        {
            Debug.Log("Added " + _item + " " + _amount + " times");

            if (ItemCollection.ToDictionary().ContainsKey(_item))
            {
                UpdateItemAmount(_item, _amount);
            }
            else
            {
                DictItem newItem = new DictItem();
                newItem.item = _item;
                newItem.amount = _amount;
                ItemCollection.items.Add(newItem);
            }
        }

        /// <summary>
        /// Remove an x amount of item from inventory.
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_amount"></param>
        public void RemoveFromInventory(ObjectDatabase _item, int _amount)
        {
            Debug.Log("Removed " + _item + " " + _amount + " times");

            if (ItemCollection.ToDictionary().ContainsKey(_item))
            {
                UpdateItemAmount(_item, -_amount);
            }
        }

        /// <summary>
        /// Update amount of item that the inventory has. By default amount is subtracted from the total.
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_amount"></param>
        private void UpdateItemAmount(ObjectDatabase _item, int _amount)
        {
            foreach (DictItem item in ItemCollection.items)
            {
                if (item.item == _item)
                {
                    item.amount -= _amount;
                }
            }
        }

        /// <summary>
        /// Get the all items from inventory without their quantities.
        /// </summary>
        /// <returns></returns>
        public ObjectDatabase[] GetItems()
        {
            return ItemCollection.ToDictionary().Keys.ToArray();
        }
    }
}
