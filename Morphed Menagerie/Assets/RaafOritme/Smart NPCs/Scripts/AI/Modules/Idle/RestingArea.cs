using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class RestingArea : BaseEnvironment
    {
        public RestingInfo restingInfo = new();

        private void Awake()
        {
            restingInfo.restId = gameObject.GetInstanceID();
            restingInfo.availableSeats = 0;

            for (int i = 0; i < restingInfo.seatAvailable.Count; i++)
            {
                restingInfo.seatAvailable[i] = true;
                restingInfo.availableSeats++;
            }
        }

        public override void Goapify()
        {
            if (!gameObject.GetComponent<RestAction>())
            {
                gameObject.AddComponent<RestAction>();
            }
        }

        /// <summary>
        /// Quick check to see if there are available spots.
        /// </summary>
        /// <returns></returns>
        public bool CanBookSeat()
        {
            return restingInfo.availableSeats > 0;
        }

        /// <summary>
        /// Returns transform of a position to rest at, if none available returns null.
        /// </summary>
        public Transform BookSeat()
        {
            for (int i = 0; i < restingInfo.seatAvailable.Count; i++)
            {
                if (restingInfo.seatAvailable[i])
                {
                    restingInfo.seatAvailable[i] = false;
                    restingInfo.availableSeats--;
                    return restingInfo.restingSpots[i].transform;
                }
            }

            return null;
        }

        /// <summary>
        /// give the seat back when done.
        /// </summary>
        public void ReturnSeat(Transform _seat)
        {
            for (int i = 0; i < restingInfo.restingSpots.Count; i++)
            {
                if (restingInfo.restingSpots[i] == _seat)
                {
                    restingInfo.seatAvailable[i] = true;
                    restingInfo.availableSeats++;
                }
            }
        }

        [Serializable]
        public struct RestingInfo
        {
            public int restId;
            public Transform entrance;

            public List<Transform> restingSpots;
            public List<bool> seatAvailable;
            public int availableSeats;
        }
    }
}
