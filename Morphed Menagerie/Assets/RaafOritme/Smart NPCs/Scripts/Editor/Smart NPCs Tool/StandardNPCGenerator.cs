using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RaafOritme.SmartNPCs
{
    public class StandardNPCGenerator : BaseNPCGenerator
    {
        protected BrainType brainType = BrainType.FSM;
        protected Modules modules;
        protected AIRoutine aiRoutine;
        protected AIRoutine aiRoutineCached;

        public override void GUIDraw()
        {
            GUILayout.Label("Each NPC comes with 2 brain parts, the performance brain and a selectable brain. Select a brain type for more information.", EditorStyles.wordWrappedLabel);
            if (brainType == BrainType.FSM)
            {
                GUILayout.Label("The FSM is a task driven brain that roulates between a certain routine, this brain is lightweight and versatile. ", EditorStyles.wordWrappedLabel);
            }
            if (brainType == BrainType.GOAP)
            {
                GUILayout.Label("The GOAP is an emotion driven brain, this is a lot heavier and more complex, and requires some fine tuning in the scene. This brain is capable " +
                    "of making decisions based on emotional state, past decisions, and past experiences. A GOAP powered NPC has a set of actions it can perform. " +
                    "With its capabilities it scans the environment for all that it can do and picks whatever suits its objective the best.", EditorStyles.wordWrappedLabel);
            }
            customStyle.DrawHorizontalGUILine();
            brainType = (BrainType)EditorGUILayout.EnumPopup(brainTypeTip, brainType);

            if (brainType == BrainType.GOAP)
            {
                if (GUILayout.Button("GOAPify Scene"))
                {
                    ShowGoapify();
                }
            }

            customStyle.DrawHorizontalGUILine();
            GUILayout.Label("Standard NPC settings", EditorStyles.boldLabel);

            base.GUIDraw();
            customStyle.DrawHorizontalGUILine();

            DrawModules();
            customStyle.DrawHorizontalGUILine();

            DrawExtraButtons();
            customStyle.DrawHorizontalGUILine();

            DrawErrors();
        }

        private void ShowGoapify()
        {
            GetWindow<SceneToGoap>("GOAPify Scene");
        }

        protected override void ApplyModuleSettings(AgentController _controller)
        {
            base.ApplyModuleSettings(_controller);
            _controller.settings.routine = aiRoutine;
        }

        private void DrawErrors()
        {
            // All the fields that could be missing
            List<string> fields = new();
            if (useSceneObject)
            {
                if (!prefab) fields.Add("Scene Object");
            }
            else
            {
                if (!prefab) fields.Add("Prefab Template");
            }
            if (!aiRoutine) fields.Add("AI Routine");
            if (!spawnLocation) fields.Add("Spawn Location");
            if ((modules & Modules.IDLE) == Modules.IDLE && !residence) fields.Add("Idle: Residence");
            if ((modules & Modules.PATROL) == Modules.PATROL && !patrolArea) fields.Add("Patrol: Patrol Area");

            if (fields.Count > 0)
            {
                GUILayout.Label("Missing fields:", EditorStyles.boldLabel);

                DrawColorField(null);
                foreach (string field in fields)
                {
                    GUILayout.Label(field);
                }
                CloseColorField();
            }

            EditorGUI.BeginDisabledGroup(fields.Count > 0);
            if (GUILayout.Button("Generate NPC"))
            {
                if (quantity > 1)
                {
                    GameObject parent = new GameObject();
                    parent.name = "Parent " + npcName;
                    for (int i = 0; i < quantity; i++)
                    {
                        GameObject npc = GenerateNPC(i);
                        npc.name = npc.name + " " + (i + 1);
                        npc.transform.parent = parent.transform;
                    }
                }
                else
                {
                    GenerateNPC(0);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        protected void DrawModules()
        {
            GUILayout.Label(optionsTip, EditorStyles.boldLabel);
            GUILayout.Label("You can find the routines in: Assets > RaafOritme > Smart NPCs > AI Data > Routines. You can create a custom one by: right clicking in there > create > RaafOritme > Smart NPCs > Create AI Routine.", EditorStyles.wordWrappedLabel);
            DrawColorField(aiRoutine);
            aiRoutine = (AIRoutine)EditorGUILayout.ObjectField(aiRoutine, typeof(AIRoutine), false);
            CloseColorField();
            if (aiRoutine != aiRoutineCached)
            {
                currentWindow = "";
            }
            aiRoutineCached = aiRoutine;

            if (aiRoutine != null)
            {
                customStyle.DrawHorizontalGUILine();
                GUILayout.BeginHorizontal();
                List<string> baseModule = new();
                modules = new();
                foreach (Container routine in aiRoutine.routine)
                {
                    if (!baseModule.Contains(routine.module.ToString()))
                    {
                        baseModule.Add(routine.module.ToString());
                        switch (routine.module)
                        {
                            case IdleModule module:
                                DrawTabButton("Idle");
                                modules |= Modules.IDLE;
                                break;
                            case PatrolModule module:
                                DrawTabButton("Patrol");
                                modules |= Modules.PATROL;
                                break;
                            case CombatModule module:
                                DrawTabButton("Combat");
                                modules |= Modules.COMBAT;
                                break;
                            case DialogueModule module:
                                DrawTabButton("Dialogue");
                                modules |= Modules.DIALOGUE;
                                break;
                            case SensoryModule module:
                                DrawTabButton("Sensory");
                                modules |= Modules.SENSORY;
                                break;
                            default:
                                break;
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                modules = new();
            }

            if (modules != 0 && currentWindow != "")
            {
                customStyle.DrawHorizontalGUILine();
            }

            switch (currentWindow)
            {
                case "Idle":
                    DrawWindowIdle();
                    break;
                case "Patrol":
                    DrawWindowPatrol();
                    break;
                case "Combat":
                    DrawWindowCombat();
                    break;
                case "Dialogue":
                    DrawWindowDialogue();
                    break;
                case "Sensory":
                    DrawWindowSensory();
                    break;
                    // TIP: Through reflection this can be made a lot more modular. This might be added in the future.
            }
        }

        protected override GameObject CreateAgent(int _iteration)
        {
            GameObject newNPC = useSceneObject ? prefab : Instantiate(prefab, Vector3.zero, Quaternion.identity);
            if (spawnLocation != null)
            {
                newNPC.transform.position = spawnLocation.position;
            }

            AgentController controller = newNPC.TryAddComponent<AgentController>();
            controller.settings = new SettingsAI();

            newNPC.name = npcName;
            newNPC.GetComponent<Entity>().SetStatistics(vitality, strength);
            newNPC.GetComponent<NavMeshAgent>().avoidancePriority = 100 - (int)(50 * (0.5f + (int)importance * 0.286f) * 0.5f); //0.285 = dividing by 3.5f
            if (brainType == BrainType.FSM)
            {
                newNPC.GetComponent<AgentController>().brainType = BrainType.FSM;
            }
            if (brainType == BrainType.GOAP)
            {
                AgentController agentController = newNPC.GetComponent<AgentController>();
                agentController.brainType = BrainType.GOAP;
                agentController.modules = modules;
            }
            Selection.activeGameObject = newNPC;

            return newNPC;
        }
    }
}
