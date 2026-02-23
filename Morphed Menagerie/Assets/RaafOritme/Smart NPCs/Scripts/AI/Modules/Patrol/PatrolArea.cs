using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class PatrolArea : BaseEnvironment
    {
        public List<PatrolNode> nodes = new();

        private List<Vector3> points = new();

        private int hitCount;
        
        public string[] filters = {
            "terrain",
            "rest area",
            "npc"
        };

        private void Awake()
        {
            AssignNodes();
        }

        public override void Goapify()
        {
            if (!gameObject.GetComponent<PatrolAction>())
            {
                gameObject.AddComponent<PatrolAction>();
            }
        }

        private void AssignNodes()
        {
            nodes.Clear();
            foreach (Transform t in transform)
            {
                PatrolNode node = t.GetComponent<PatrolNode>();

                if (node != null)
                {
                    nodes.Add(node);
                }
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            points.Clear();

            if (nodes.Count == 0)
            {
                AssignNodes();

                if (nodes.Count == 0)
                {
                    return;
                }
            }

            // connect all other nodes
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                points.Add(nodes[i].transform.position);
                points.Add(nodes[i + 1].transform.position);
            }

            // connect starting and end node
            points.Add(nodes[0].transform.position);
            points.Add(nodes[nodes.Count - 1].transform.position);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLineList(points.ToArray());
        }

        /// <summary>
        /// Method used to double check if all waypoints are reachable directly.
        /// </summary>
        [Button("Validate")]
        public void ValidateWaypoints()
        {
            AssignNodes();
            hitCount = 0;

            for (int i = 0; i < nodes.Count - 1; ++i)
            {
                if (i == 0)
                {
                    CheckForObject(nodes[0], nodes[nodes.Count - 1]);
                }
                else
                {
                    CheckForObject(nodes[i], nodes[i - 1]);
                }
            }

            if (hitCount == 0)
            {
                Debug.Log("No object obstacles found!");
            }
        }

        private void CheckForObject(PatrolNode _origin, PatrolNode _target)
        {
            Vector3 origin = _origin.gameObject.transform.position;
            Vector3 target = _target.gameObject.transform.position;
            Vector3 direction = (target - origin);
            RaycastHit hit;

            if (Physics.Raycast(origin, direction, out hit, Vector3.Distance(origin, target)))
            {
                if (PassFilter(hit.transform.name))
                {
                    Debug.Log($"Detected object {hit.transform.name} between {_origin.name} and {_target.name}", hit.transform);
                    hitCount++;
                }
            }
        }

        private bool PassFilter(string _name)
        {
            foreach (string filter in filters)
            {
                if (_name.ToLower().Contains(filter))
                {
                    return false;
                }
            }

            return true;
        }
    }
#endif
}
