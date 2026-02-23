using System;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [Serializable]
    public struct Node
    {
        public int id;
        [HideInInspector]
        public Transform area;
        public AnimationType animationType;
        [Tooltip("Can also be look direction")]
        public Transform interactObject;
        // TIP: This time can be similar to the animation time or a statemachine can be implemented for more specific routines.
        //[MinMaxSlider(1, 50)]
        public Vector2Int interactTime;
        [Tooltip("The actual position per npc will vary from the center + range in order to have random positions so that it feels more dynamic")]
        //[MinMaxSlider(1, 20)]
        public Vector2Int nodeSpread;
        [Tooltip("A higher priority increases the chance of the npc patrolling this area going straight to this node")]
        public int priority;
    }
}
