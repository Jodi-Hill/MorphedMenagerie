using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [RequireComponent(typeof(PatrolArea))]
    public class PatrolAction : BaseAction
    {
        private PatrolArea patrolArea;

        public PatrolAction()
        {
            actionName = "Patrol";
            actionCost = 10.0f;
            requiredModule = Modules.PATROL;
        }

        public override void OnEnter(BrainGOAP _npc)
        {
            if (!CanExecute(_npc))
            {
                Debug.LogError("Can no longer execute action!", _npc.gameObject);
                return;
            }

            PatrolModule patrolMod = null;
            foreach (BaseModule module in _npc.AllModules)
            {
                if (module.GetType() == typeof(PatrolModule))
                {
                    patrolMod = (PatrolModule)module;
                    break;
                }
            }

            if (patrolArea == null)
            {
                patrolArea = GetComponent<PatrolArea>();
            }

            if (patrolMod != null)
            {
                patrolMod.OnEnter(true);
                patrolMod.SetPatrolInfo(patrolArea);
            }
        }

        public override void Execute(BrainGOAP _npc)
        {
            PatrolModule patrolMod = null;
            foreach (BaseModule module in _npc.AllModules)
            {
                if (module.GetType() == typeof(PatrolModule))
                {
                    patrolMod = (PatrolModule)module;
                    patrolMod.patrolSettings.patrolAreas.Clear();
                    patrolMod.patrolSettings.patrolAreas.Add(patrolArea);
                    break;
                }
            }

            if (patrolMod != null)
            {
                patrolMod.UpdateState(_npc);
            }
        }
    }
}
