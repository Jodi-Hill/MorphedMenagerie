using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RaafOritme.SmartNPCs
{
    [Serializable]
    public class SettingsAI
    {
        public AIRoutine routine;
        public Movement movement = new();
        public Idle idle = new();
        public Patrol patrol = new();
        public Combat combat = new();
        public Dialogue dialogue = new();
        public Sensory sensory = new();
    }

    [Serializable]
    public struct Movement
    {
        public float walkSpeed;
        public float runSpeed;
    }

    [Serializable]
    public struct Idle
    {
        public Transform residence;
        public MinMaxFloat restingTimeRange;
        public LayerMask restAreaMask;
        public float maxScanRadius;
    }

    [Serializable]
    public struct Patrol
    {
        public bool randomNodes;
        public int prioritySensitivity;
        public int patrolAreaIndex;
        public List<PatrolArea> patrolAreas;
        public UnityEvent actionAfterPatrolArea;
        public UnityEvent actionAfterPatrolNode;
        public MinMaxInt nodesBeforeRestRange;
        public float maxNodeDistance;
    }

    [Serializable]
    public struct Combat
    {
        public Vitality vitality;
        public Vitality strength;
        public UnityEvent actionDuringCombat;
        public UnityEvent actionAfterCombat;
        public float aggressionTime;
        public float combatDistance;
    }

    [Serializable]
    public struct Dialogue
    {
        public float talkTime;
        public UnityEvent actionAfterDialogue;
    }

    [Serializable]
    public struct Sensory
    {
        public float effectiveObservatoryRange;
    }
}
