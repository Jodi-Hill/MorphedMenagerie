using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class MotherAI : MonoBehaviour
    {
        private Transform playerTransform;

        public static MotherAI Instance { get; private set; }
        public float performanceDistance = 30f;
        public float maxPerfDistance = 50f;
        public List<NPCDistance> agents = new();
        private List<NPCDistance> performanceAgents = new();

        private float squaredPerfDist = 400f;
        private float squaredMaxPerfDist = 1000f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            squaredPerfDist = performanceDistance * performanceDistance;
            squaredMaxPerfDist = maxPerfDistance * maxPerfDistance;

            foreach (AgentController agent in FindObjectsByType<AgentController>(FindObjectsSortMode.None))
            {
                agents.Add(new NPCDistance(QuickDistance(agent.transform), agent));
            }
        }

        private void Update()
        {
            StartCoroutine(UpdateMain());
        }

        private void FixedUpdate()
        {
            StartCoroutine(UpdatePerf());
        }

        private IEnumerator UpdateMain()
        {
            if (agents.Count == 0)
            {
                yield break;
            }

            // Update all positions and main brains
            performanceAgents.Clear();
            foreach (NPCDistance agent in agents)
            {
                agent.squaredDistance = QuickDistance(agent.npc.transform);

                if (agent.squaredDistance < squaredPerfDist)
                {
                    agent.npc.UpdateMain();
                }
                else
                {
                    performanceAgents.Add(agent);
                }
            }
            yield return null;
        }

        private IEnumerator UpdatePerf()
        {
            if (performanceAgents.Count == 0)
            {
                yield break;
            }

            // Update all performance brains from the remaining agents
            foreach (NPCDistance agent in performanceAgents)
            {
                if (agent.squaredDistance <= squaredMaxPerfDist)
                {
                    agent.npc.UpdatePerformance(true);
                }
                else
                {
                    agent.npc.UpdatePerformance(false);
                }
            }
            yield return null;
        }

        private float QuickDistance(Transform _startTransform)
        {
            return _startTransform.SquaredDistance(playerTransform);
        }
    }

    // This is essentially a dictionary that has been serialized in the Unity inspector
    [System.Serializable]
    public class NPCDistance
    {
        public float squaredDistance;
        public AgentController npc;

        public NPCDistance(float _squaredDistance, AgentController _npc)
        {
            squaredDistance = _squaredDistance;
            npc = _npc;
        }
    }
}
