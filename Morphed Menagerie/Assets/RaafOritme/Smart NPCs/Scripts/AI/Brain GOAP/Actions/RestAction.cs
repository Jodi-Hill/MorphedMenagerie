using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [RequireComponent(typeof(RestingArea))]
    public class RestAction : BaseAction
    {
        private RestingArea restArea;

        public RestAction()
        {
            actionName = "Rest";
            actionCost = 0.0f;
            requiredModule = Modules.IDLE;
        }

        public override void OnEnter(BrainGOAP _npc)
        {
            if (!CanExecute(_npc))
            {
                Debug.LogError("Can no longer execute action!", _npc.gameObject);
                return;
            }

            IdleModule idleMod = null;

            foreach (BaseModule module in _npc.AllModules)
            {
                if (module.GetType() == typeof(IdleModule))
                {
                    idleMod = (IdleModule)module;
                    break;
                }
            }

            if (restArea == null)
            {
                restArea = GetComponent<RestingArea>();
            }

            if (idleMod != null)
            {
                idleMod.OnEnter(true);
                idleMod.SetRestingInfo(restArea);
            }
        }

        public override bool CanExecute(BrainGOAP _npc)
        {
            if (restArea == null)
            {
                restArea = GetComponent<RestingArea>();
            }

            if (!base.CanExecute(_npc) || !restArea.CanBookSeat())
            {
                return false;
            }

            return true;
        }

        public override void Execute(BrainGOAP _npc)
        {
            IdleModule idleMod = null;
            foreach (BaseModule module in _npc.AllModules)
            {
                if (module.GetType() == typeof(IdleModule))
                {
                    idleMod = (IdleModule)module;
                    break;
                }
            }

            if (idleMod != null)
            {
                idleMod.UpdateState(_npc);
            }
        }
    }
}
