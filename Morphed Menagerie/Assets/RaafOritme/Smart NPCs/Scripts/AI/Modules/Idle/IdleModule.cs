using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class IdleModule : BaseModule
    {
        private float timer;

        private IdleState idleState;
        private RestingArea restArea;
        private Transform entrance;
        private Transform seat;
        private RestingObject restingObject; // TIP: This can be used for interactions with specific objects.

        private Collider[] hitInfo = new Collider[5];
        private HashSet<Collider> nearbyAreas;

        private float graceWait = 1f;
        private float maxIdleTime;

        private float restChance = 0.8f; // higher value (max 1.0) increases the chance of NPC resting at a rest spot instead of the residence when it can choose

        public override void Initialize(AgentController _agentController)
        {
            agentController = _agentController;
        }

        public override void OnEnter(bool _excludeAction = false)
        {
            entrance = null;
            graceWait = 1f;
            maxIdleTime = 30f;
            timer = Random.Range(agentController.settings.idle.restingTimeRange.min, agentController.settings.idle.restingTimeRange.max);

            if (!_excludeAction)
            {
                ChooseRestingPlace();
            }
        }

        public override void UpdateState(IBrain _brain)
        {
            maxIdleTime -= Time.deltaTime;
            if (maxIdleTime <= 0f)
            {
                FinishedResting();
                return;
            }

            if ((idleState & IdleState.RESTING) == IdleState.RESTING)
            {
                RestingPlace();
            }
            else
            {
                if (entrance)
                {
                    GoingToRestPlace();
                }
                else
                {
                    GoingToHome();
                }
            }
        }

        /// <summary>
        /// Apply resting info for agent to iterate automatically through the proces.
        /// </summary>
        /// <param name="_restingArea"></param>
        public void SetRestingInfo(RestingArea _restingArea)
        {
            restArea = _restingArea;
            entrance = restArea.restingInfo.entrance;
            seat = restArea.BookSeat();
            idleState = IdleState.NAVIGATING;
            // TIP: restingObject can be set here similarly to the fields above.
        }

        /// <summary>
        /// Searches nearby for a resting place and decides where to rest.
        /// </summary>
        private void ChooseRestingPlace()
        {
            nearbyAreas ??= new HashSet<Collider>();
            nearbyAreas.Clear();

            int objectsInRangeCount = agentController.transform.CollidersInRange(agentController.settings.idle.maxScanRadius, ref hitInfo, agentController.settings.idle.restAreaMask);
            for (int i = 0; i < objectsInRangeCount; i++)
            {
                nearbyAreas.Add(hitInfo[i]);
            }

            if (nearbyAreas.Count > 0)
            {
                // Local function used for quickly finding closest by resting area
                Collider GetDistanceFromPoint(Collider _result, Collider _location)
                {
                    return (agentController.transform.position - _location.transform.position).sqrMagnitude <
                        (agentController.transform.position - _result.transform.position).sqrMagnitude ? _location : _result;
                }
                Collider nearbyRestPlace = nearbyAreas.Aggregate(GetDistanceFromPoint);

                // If there is a place to rest nearby the agent has x% chance of going there
                if (nearbyRestPlace && Random.value < restChance)
                {
                    restArea = nearbyRestPlace.GetComponent<RestingArea>();
                    entrance = restArea.restingInfo.entrance;
                }
                idleState = IdleState.NAVIGATING;
            }
        }

        /// <summary>
        /// Agent goes to a place to rest.
        /// Once at the place it tries to find an empty seat otherwise it returns to home.
        /// </summary>
        private void GoingToRestPlace()
        {
            if (!agentController.pathfinding.CanNavigate())
            {
                return;
            }

            agentController.pathfinding.SetDestination(entrance.position);
            if (agentController.transform.WithinRange(entrance.position, agentController.settings.patrol.maxNodeDistance))
            {
                seat = restArea.BookSeat();

                if (seat)
                {
                    idleState = IdleState.RESTING;
                }
                else
                {
                    entrance = null;
                }
            }
        }

        /// <summary>
        /// Agent goes to home to rest. 
        /// </summary>
        private void GoingToHome()
        {
            if (!agentController.pathfinding.CanNavigate())
            {
                return;
            }

            agentController.pathfinding.SetDestination(agentController.settings.idle.residence.position);
            if (agentController.transform.WithinRange(agentController.settings.idle.residence.position, agentController.settings.patrol.maxNodeDistance))
            {
                idleState = IdleState.RESTING;
            }
        }

        /// <summary>
        /// Resting behaviour of agent.
        /// </summary>
        private void RestingPlace()
        {
            if (!agentController.pathfinding.CanNavigate())
            {
                return;
            }

            if (seat)
            {
                if (!agentController.transform.WithinRange(seat.position, agentController.settings.patrol.maxNodeDistance))
                {
                    agentController.pathfinding.SetDestination(seat.position);
                    return;
                }
            }
            else
            {
                if (!agentController.transform.WithinRange(agentController.settings.idle.residence.position, agentController.settings.patrol.maxNodeDistance))
                {
                    agentController.pathfinding.SetDestination(agentController.settings.idle.residence.position);
                    return;
                }
            }

            if (graceWait > 0)
            {
                graceWait -= Time.deltaTime;
                return;
            }

            agentController.pathfinding.SetDestination(agentController.transform.position);

            // Performing resting animations
            if (seat != null && restingObject == null)
            {
                restingObject = seat.GetComponent<RestingObject>();

                if (restingObject.interactObject != null)
                {
                    agentController.transform.LookAt(restingObject.interactObject);
                }

                // A better way of handling animations can be used.
                switch (restingObject.animationType)
                {
                    case AnimationType.SLEEPING:
                        agentController.AnimationHandler.SetTrigger("Sleeping");
                        break;
                    case AnimationType.SITTING:
                        agentController.AnimationHandler.SetTrigger("Sitting");
                        break;
                    case AnimationType.EATING:
                    case AnimationType.DRINKING:
                        agentController.AnimationHandler.SetTrigger("Drinking");
                        break;
                    case AnimationType.LISTENING:
                    case AnimationType.CHATTING:
                        agentController.AnimationHandler.SetTrigger("Talking");
                        break;
                }
            }

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                FinishedResting();
            }
        }

        private void FinishedResting()
        {
            if (seat)
            {
                restArea.ReturnSeat(seat);
                restingObject = null;
                seat = null;
            }

            agentController.mainBrain.TransitionToNext();
        }

        public override void OnExit()
        {
            timer = 0;
            if (seat)
            {
                restArea.ReturnSeat(seat);
            }
            graceWait = 1f;
            restingObject = null;
            seat = null;
            entrance = null;
            idleState = IdleState.NONE;
        }
    }
}
