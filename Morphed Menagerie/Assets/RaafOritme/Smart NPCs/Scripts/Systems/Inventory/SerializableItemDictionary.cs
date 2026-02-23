using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// This is used in order to serialize it in the Unity inspector.
    /// </summary>
    [Serializable]
    public class SerializableItemDictionary
    {
        [SerializeField]
        public ItemDictionary inventory;
    }

    [Serializable]
    public class ItemDictionary
    {
        [SerializeField]
        public List<DictItem> items = new List<DictItem>();

        /// <summary>
        /// Convert the list to an actual dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<ObjectDatabase, int> ToDictionary()
        {
            Dictionary<ObjectDatabase, int> newDict = new Dictionary<ObjectDatabase, int>();

            foreach (DictItem dictItem in items)
            {
                newDict.Add(dictItem.item, dictItem.amount);
            }

            return newDict;
        }
    }

    [Serializable]
    public class DictItem
    {
        [SerializeField]
        public ObjectDatabase item;

        [SerializeField]
        public int amount;
    }
}
