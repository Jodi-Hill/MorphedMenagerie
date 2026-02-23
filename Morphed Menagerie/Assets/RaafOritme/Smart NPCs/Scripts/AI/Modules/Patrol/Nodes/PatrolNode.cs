using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class PatrolNode : MonoBehaviour
    {
        public Node node;

        private void Start()
        {
            node.id = gameObject.GetInstanceID();
            node.area = transform.parent;

            NodeManager.Instance.AddNode(node);
        }
    }
}
