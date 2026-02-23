using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class NodeManager : MonoBehaviour
    {
        private static Dictionary<int, Node> nodeCollection = new();

        public static NodeManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            nodeCollection.Clear();
        }

        /// <summary>
        /// Add a new node to the lookup table by instance id.
        /// </summary>
        public void AddNode(Node _nodeInfo)
        {
            if (nodeCollection.ContainsKey(_nodeInfo.id))
            {
                Debug.LogError("A new node was being added with an existing key: " + _nodeInfo.id);
                return;
            }

            nodeCollection.Add(_nodeInfo.id, _nodeInfo);
        }

        /// <summary>
        /// Get node by instance id from the lookup table.
        /// </summary>
        public Node GetNodeById(int id)
        {
            Node node = new();
            nodeCollection.TryGetValue(id, out node);

            if (!nodeCollection.ContainsKey(id))
            {
                Debug.LogError("A node was requested that doesnt exist with key: " + id);
            }

            return node;
        }
    }
}
