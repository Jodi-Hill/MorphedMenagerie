using System.Collections.Generic;

namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// Adds a non-existent item to a list.
    /// </summary>
    public static class ListExtensions
    {
        public static void TryAdd<T>(this List<T> _list, T _item)
        {
            if (!_list.Contains(_item))
            {
                _list.Add(_item);
            }
        }
    }
}
