using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RaafOritme.SmartNPCs
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AnimationHandler))]
    public class AgentController : MonoBehaviour
    {
        [Header("Debug Area")]
        public OverRuleState overRule = OverRuleState.NONE;
        public string currentAction;
        public List<string> actionStack;
        public Inventory agentInventory = new();

        [Header("Settings Area")]
        public GameObject mainVisualObject;
        public GameObject performanceVisualObject;
        public Collider npcCollider;
        public AbstractBrain mainBrain;
        public BrainType brainType;
        [HideInInspector]
        public Modules modules = new(); // only used for GOAP

        private PerformanceBrain performanceBrain;

        private NavMeshAgent navmeshAgent;
        private NavMeshAgent NavmeshAgent => gameObject.GetAndCacheComponent(ref navmeshAgent);
        private NavMeshObstacle navmeshObstacle;
        private NavMeshObstacle NavmeshObstacle => gameObject.GetAndCacheComponent(ref navmeshObstacle);
        private AnimationHandler animationHandler;
        public AnimationHandler AnimationHandler => gameObject.GetAndCacheComponent(ref animationHandler);
        public SettingsAI settings;
        public Animator animator;
        public Pathfinding pathfinding;

        private void Start()
        {
            ResetSystem();
        }

        /// <summary>
        /// Can be used to reset the AI system.
        /// </summary>
        public void ResetSystem()
        {
            if (npcCollider == null)
            {
                npcCollider = GetComponent<Collider>();
            }

            pathfinding = GetComponent<Pathfinding>();
            if (pathfinding == null)
            {
                pathfinding = gameObject.AddComponent<Pathfinding>();
            }

            switch (brainType)
            {
                default:
                case BrainType.FSM:
                    mainBrain = new BrainFSM();
                    break;
                case BrainType.GOAP:
                    mainBrain = new BrainGOAP();
                    break;
            }
            performanceBrain = new PerformanceBrain();

            mainBrain.agentController = this;
            StartCoroutine(DelayBrainStart());
        }

        /// <summary>
        /// At start up the action planner gets populated during the awake and start methods, so this has to be a frame after that frame.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayBrainStart()
        {
            yield return new WaitForEndOfFrame();

            mainBrain.Initialize(settings.routine, gameObject, this);
            performanceBrain.agentController = this;
            NavmeshAgent.avoidancePriority = settings.patrol.prioritySensitivity;
            NavmeshAgent.speed = settings.movement.walkSpeed;
            npcCollider.enabled = true;
            mainBrain.Start();
        }

        /// <summary>
        /// Update the main brain logic of the AI system, this happens every frame.
        /// </summary>
        public void UpdateMain()
        {
            currentAction = mainBrain.currentAction;
            actionStack = mainBrain.lastActions;

            if (!NavmeshAgent.enabled)
            {
                NavmeshAgent.enabled = true;
            }

            if (!mainVisualObject.activeSelf)
            {
                mainVisualObject.SetActive(true);
            }
            if (performanceVisualObject.activeSelf)
            {
                performanceVisualObject.SetActive(false);
            }

            if (overRule == OverRuleState.DECEASED || overRule == OverRuleState.CHATTING)
            {
                if (!NavmeshObstacle.enabled)
                {
                    NavmeshObstacle.enabled = true;
                }

                if (navmeshAgent.enabled)
                {
                    navmeshAgent.enabled = false;
                }
                return;
            }
            else if (NavmeshObstacle.enabled)
            {
                NavmeshObstacle.enabled = false;
                navmeshAgent.enabled = true;
            }

            mainBrain.Update();
            UpdateAnimator();
        }

        /// <summary>
        /// Update the AI through performance mode since it is not required to be fully simulated.
        /// </summary>
        public void UpdatePerformance(bool _calculations)
        {
            if (NavmeshAgent.enabled)
            {
                NavmeshAgent.enabled = false;
            }

            if (mainVisualObject.activeSelf)
            {
                mainVisualObject.SetActive(false);
            }
            if (!performanceVisualObject.activeSelf)
            {
                performanceVisualObject.SetActive(true);
            }

            if (_calculations)
            {
                performanceBrain.Update();
            }
        }

        /// <summary>
        /// Assigns a specific Over Rule. This means that it can immediately switch from action A to action B.
        /// </summary>
        /// <param name="_overRule"></param>
        public void SetOverrule(OverRuleState _overRule)
        {
            overRule = _overRule;
            mainBrain.TransitionToState(overRule);

            switch (_overRule)
            {
                default:
                    EnableNavMesh();
                    npcCollider.enabled = true;
                    enabled = true;
                    AnimationHandler.BackToDefault();
                    break;

                case OverRuleState.DECEASED:
                    DisableNavMesh();
                    npcCollider.enabled = false;
                    AnimationHandler.SetSpeed(0);
                    enabled = false;
                    AnimationHandler.SetTrigger("Death");
                    break;

                case OverRuleState.CHATTING:
                    DisableNavMesh();
                    AnimationHandler.SetSpeed(0);
                    AnimationHandler.SetTrigger("Talking");
                    break;
            }
        }

        private void EnableNavMesh()
        {
            if (!NavmeshAgent.enabled)
            {
                NavmeshAgent.enabled = true;
            }
            if (NavmeshObstacle.enabled)
            {
                NavmeshObstacle.enabled = false;
            }
        }

        private void DisableNavMesh()
        {
            if (NavmeshAgent.enabled)
            {
                NavmeshAgent.enabled = false;
            }
            if (!NavmeshObstacle.enabled)
            {
                NavmeshObstacle.enabled = true;
            }
        }

        /// <summary>
        /// Returns the Over Rule states.
        /// </summary>
        /// <returns></returns>
        public OverRuleState GetOverrule()
        {
            return overRule;
        }

        private void UpdateAnimator()
        {
            AnimationHandler.SetSpeed(navmeshAgent.velocity.magnitude);
        }
    }
}
