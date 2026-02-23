using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public abstract class BaseNPCGenerator : EditorWindow
    {
        // NPC basics
        protected string npcName = "NPC AI";
        protected Inventory startingInventory = new();
        protected int quantity = 1;
        protected InspectorHeightOptions importance = InspectorHeightOptions.Average;
        protected bool useSceneObject = false;
        protected GameObject prefab;
        protected Transform spawnLocation;

        // NPC module settings
        protected Vitality vitality = Vitality.MODERATE;
        protected Vitality strength = Vitality.WEAK;
        protected int walkSpeed = 2;
        protected int runSpeed = 5;
        protected InspectorLengthOptions aggressionTime = InspectorLengthOptions.Average;
        protected InspectorDistanceOptions combatDistance = InspectorDistanceOptions.Average;
        protected Transform residence;
        protected MinMaxFloat restTimeRange = new MinMaxFloat() { min = 2, max = 8 };
        protected LayerMask restAreaMask;
        protected bool randomNodes = false;
        protected PatrolArea patrolArea;
        protected MinMaxInt nodeAmountRange = new MinMaxInt() { min = 4, max = 10 };
        protected InspectorDistanceOptions nodeDistance = InspectorDistanceOptions.Average;
        protected InspectorHeightOptions prioritySensitivity = InspectorHeightOptions.Average;
        protected InspectorDistanceOptions maxScanDistance = InspectorDistanceOptions.Average;

        // Tooltips
        protected GUIContent brainTypeTip = new GUIContent("Brain type", "Which brain type should this NPC use?");
        protected GUIContent importanceTip = new GUIContent("Set priority", "How important is this NPC? Lower value implies higher importance");
        protected GUIContent quantityTip = new GUIContent("Set amount", "How many copies should be generated? It will receive a parent when it is more than 1");
        protected GUIContent residenceTip = new GUIContent("Residence", "The world location where the NPC should go when it is planning to rest");
        protected GUIContent spawnTip = new GUIContent("Spawn position", "When set the NPCs wont spawn in the Vector3.zero position");
        protected GUIContent optionsTip = new GUIContent("Module settings", "Select all the routine you would like to use and set the settings");
        protected GUIContent restingTimeMinTip = new GUIContent("Resting time min", "How long should the npc rest in general");
        protected GUIContent restingTimeMaxTip = new GUIContent("Resting time max", "How long should the npc rest in general");
        protected GUIContent restingLayerTip = new GUIContent("Resting area layer", "Select the layer in which the npc can find resting areas. You can create a new layer for this if you havent done so yet through the layer menu in Unity");
        protected GUIContent randomNodeTip = new GUIContent("Random nodes", "Should the npc follow a linair path or should it patrol randomly between the nodes");
        protected GUIContent patrolAreaTip = new GUIContent("Patrol Area", "Give a base patrol area for the npc. You can also do this after creating the npc");
        protected GUIContent nodeAmountMinTip = new GUIContent("Node amount min", "How many nodes should the npc patrol before resting");
        protected GUIContent nodeAmountMaxTip = new GUIContent("Node amount max", "How many nodes should the npc patrol before resting");
        protected GUIContent nodeDistanceTip = new GUIContent("Node distance", "How close does the npc need to be to a node before it has been reached? If it is set to low the npc might never reach it!");
        protected GUIContent priorityTip = new GUIContent("Priority sensitivity", "How high should the odds be for a npc going to a waypoint with a higher priority? A higher score means a lower chance");
        protected GUIContent scanDistanceTip = new GUIContent("Max scan distance", "How far should the agent be looking for a place to rest? This is from 15 to roughly 75 units");
        protected GUIContent baseNameTip = new GUIContent("Name in hierarchy", "What should the base name of a npc be");
        protected GUIContent modulesTip = new GUIContent("Active modules", "Which modules should be used on this system?");
        protected GUIContent routineTip = new GUIContent("Behaviour routine", "What should the routine look like?");
        protected GUIContent prefabTip = new GUIContent("Prefab template", "What template will the npc be based on");
        protected GUIContent sceneObjectTip = new GUIContent("Use scene object", "Instead of creating a new gameobject, use an existing one from the scene");
        protected GUIContent walkSpeedTip = new GUIContent("Walk speed", "How fast should the npc walk");
        protected GUIContent runSpeedTip = new GUIContent("Run speed", "How fast should the npc run");
        protected GUIContent aggressionTimeTip = new GUIContent("Aggression time", "How long should the npc remain aggressive for");
        protected GUIContent combatDistanceTip = new GUIContent("Combat distance", "What is the minimal combat distance for the npc");
        protected GUIContent vitalityTip = new GUIContent("Vitality", "How healthy is the npc");
        protected GUIContent strengthTip = new GUIContent("Strength", "How strong is the npc");
        protected GUIContent sceneObjectPrefab = new GUIContent("Scene object", "Which scene object should receive all the logic?");

        // Field related things
        protected Color red = new Color(0.4f, 0.2f, 0.2f);
        protected GUIStyles customStyle = new();

        // Define custom GUIStyles for tab buttons
        protected string currentWindow;

        /// <summary>
        /// Creates the NPC in the hierarchy. This is repeated an x amount of times based on the parameter.
        /// </summary>
        /// <param name="_iteration"></param>
        /// <returns></returns>
        protected abstract GameObject CreateAgent(int _iteration);

        /// <summary>
        /// Generates the NPC it self with all the required settings.
        /// </summary>
        /// <param name="_iteration"></param>
        /// <returns></returns>
        protected GameObject GenerateNPC(int _iteration)
        {
            GameObject newNPC = CreateAgent(_iteration);
            AgentController controller = newNPC.GetComponent<AgentController>();
            ApplyModuleSettings(controller);
            return newNPC;
        }

        /// <summary>
        /// Draws the interface with all the fields.
        /// </summary>
        public virtual void GUIDraw()
        {
            npcName = EditorGUILayout.TextField(baseNameTip, npcName);
            quantity = EditorGUILayout.IntField(quantityTip, quantity);
            useSceneObject = EditorGUILayout.Toggle(sceneObjectTip, useSceneObject);

            DrawColorField(prefab);
            if (useSceneObject)
            {
                prefab = EditorGUILayout.ObjectField(sceneObjectPrefab, prefab, typeof(GameObject), true) as GameObject;
            }
            else
            {
                prefab = EditorGUILayout.ObjectField(prefabTip, prefab, typeof(GameObject), false) as GameObject;
            }
            CloseColorField();

            DrawColorField(spawnLocation);
            spawnLocation = EditorGUILayout.ObjectField(spawnTip, spawnLocation, typeof(Transform), true) as Transform;
            CloseColorField();

            customStyle.DrawHorizontalGUILine();
            GUILayout.Label("NPC statistics", EditorStyles.boldLabel);
            importance = (InspectorHeightOptions)EditorGUILayout.EnumPopup(importanceTip, importance);
            walkSpeed = EditorGUILayout.IntField(walkSpeedTip, walkSpeed);
            runSpeed = EditorGUILayout.IntField(runSpeedTip, runSpeed);
        }

        /// <summary>
        /// Applies every single setting.
        /// </summary>
        /// <param name="_controller"></param>
        protected virtual void ApplyModuleSettings(AgentController _controller)
        {
            _controller.agentInventory = startingInventory;

            // Movement settings
            _controller.settings.movement.walkSpeed = walkSpeed;
            _controller.settings.movement.runSpeed = runSpeed;

            // IdleModule settings
            _controller.settings.idle.residence = residence;
            _controller.settings.idle.restingTimeRange = restTimeRange;
            _controller.settings.idle.restAreaMask = 1 << restAreaMask;
            _controller.settings.idle.maxScanRadius = 20 * (0.5f + (int)maxScanDistance * 0.286f); //0.285 = dividing by 3.5f

            // PatrolModule settings
            _controller.settings.patrol.randomNodes = randomNodes;
            _controller.settings.patrol.prioritySensitivity = 100 - (int)(50 * (0.5f + (int)prioritySensitivity * 0.286f) * 0.5f);
            if (patrolArea)
            {
                if (_controller.settings.patrol.patrolAreas == null)
                {
                    _controller.settings.patrol.patrolAreas = new();
                }
                _controller.settings.patrol.patrolAreas.Add(patrolArea);
            }
            _controller.settings.patrol.nodesBeforeRestRange = nodeAmountRange;
            _controller.settings.patrol.maxNodeDistance = (0.5f + (int)nodeDistance) * 0.5f;

            // CombatModule settings
            _controller.settings.combat.aggressionTime = 20 * (0.5f + (int)aggressionTime * 0.286f); //0.285 = dividing by 3.5f
            _controller.settings.combat.combatDistance = (1 + (int)combatDistance) * 0.5f;
            _controller.settings.combat.vitality = vitality;
            _controller.settings.combat.strength = strength;
        }

        /// <summary>
        /// Draws the buttons for the tabs in the interface.
        /// </summary>
        /// <param name="_label"></param>
        protected void DrawTabButton(string _label)
        {
            GUIStyle style = currentWindow == _label ? customStyle.selectedStyle : customStyle.unselectedStyle;
            if (GUILayout.Button(_label, style))
            {
                if (_label != currentWindow)
                {
                    currentWindow = _label;
                }
                else
                {
                    currentWindow = "";
                }
            }
        }

        /// <summary>
        /// Marks an entire field with a certain color, must use CloseColorField() as well.
        /// </summary>
        /// <param name="_value">Optional parameter for the check</param>
        /// <param name="_baseColor">Optional parameter with a base value of red</param>
        protected void DrawColorField(System.Object _value, Color? _baseColor = null)
        {
            if (_baseColor == null)
            {
                _baseColor = red;
            }

            Rect screenRect = GUILayoutUtility.GetRect(1, 1);
            Rect vertRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y, screenRect.width + 17, vertRect.height + 5), (_value != null) ? Color.clear : red);
        }

        protected void CloseColorField()
        {
            EditorGUILayout.EndVertical();
        }

        #region Draw every single module based window
        protected void DrawWindowIdle()
        {
            GUILayout.Label("Idle Module", EditorStyles.boldLabel);
            GUILayout.Label("This modules gives the AI the capabilities of resting. When tired it will either go to the designated residence or dynamically look for a place within vicinity to rest at.", EditorStyles.wordWrappedLabel);
            customStyle.DrawHorizontalGUILine();

            DrawColorField(residence);
            residence = (Transform)EditorGUILayout.ObjectField(residenceTip, residence, typeof(Transform));
            CloseColorField();

            GUILayout.BeginHorizontal();
            restTimeRange.min = EditorGUILayout.FloatField(restingTimeMinTip, restTimeRange.min);
            restTimeRange.max = EditorGUILayout.FloatField(restingTimeMaxTip, restTimeRange.max);
            GUILayout.EndHorizontal();
            restAreaMask = EditorGUILayout.LayerField(restingLayerTip, restAreaMask);
            maxScanDistance = (InspectorDistanceOptions)EditorGUILayout.EnumPopup(scanDistanceTip, maxScanDistance);
        }

        protected void DrawWindowPatrol()
        {
            GUILayout.Label("Patrol Module", EditorStyles.boldLabel);
            GUILayout.Label("This modules gives the AI the capabilities of patrolling. The NPC will be able to navigate between waypoints. In performance mode the NPC will use a lightweight method for navigating between the waypoints.", EditorStyles.wordWrappedLabel);
            customStyle.DrawHorizontalGUILine();

            randomNodes = EditorGUILayout.Toggle(randomNodeTip, randomNodes);
            prioritySensitivity = (InspectorHeightOptions)EditorGUILayout.EnumPopup(priorityTip, prioritySensitivity);

            DrawColorField(patrolArea);
            patrolArea = EditorGUILayout.ObjectField(patrolAreaTip, patrolArea, typeof(PatrolArea), true) as PatrolArea;
            CloseColorField();

            GUILayout.BeginHorizontal();
            nodeAmountRange.min = EditorGUILayout.IntField(nodeAmountMinTip, nodeAmountRange.min);
            nodeAmountRange.max = EditorGUILayout.IntField(nodeAmountMaxTip, nodeAmountRange.max);
            GUILayout.EndHorizontal();
            nodeDistance = (InspectorDistanceOptions)EditorGUILayout.EnumPopup(nodeDistanceTip, nodeDistance);
        }

        protected void DrawWindowCombat()
        {
            GUILayout.Label("Combat Module", EditorStyles.boldLabel);
            GUILayout.Label("This modules gives the AI the capabilities of combat. This is a very simple combat system that is used as an example.", EditorStyles.wordWrappedLabel);
            customStyle.DrawHorizontalGUILine();

            aggressionTime = (InspectorLengthOptions)EditorGUILayout.EnumPopup(aggressionTimeTip, aggressionTime);
            combatDistance = (InspectorDistanceOptions)EditorGUILayout.EnumPopup(combatDistanceTip, combatDistance);
            vitality = (Vitality)EditorGUILayout.EnumPopup(vitalityTip, vitality);
            strength = (Vitality)EditorGUILayout.EnumPopup(strengthTip, strength);
        }

        protected void DrawWindowDialogue()
        {
            GUILayout.Label("Dialogue Module", EditorStyles.boldLabel);
            GUILayout.Label("This modules gives the AI the capabilities of dialogue. NPCs without this module can not have a conversation with each other or the player.", EditorStyles.wordWrappedLabel);
        }

        protected void DrawWindowSensory()
        {
            GUILayout.Label("Sensory Module", EditorStyles.boldLabel);
            GUILayout.Label("This modules gives the AI the capabilities of sensory. Sensory behaviour are elements based on observation, for example a NPC with this module can hear an explosion or see someone getting attacked.", EditorStyles.wordWrappedLabel);
        }
        #endregion

        protected void DrawExtraButtons()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("New Rest Area"))
            {
                ShowRestWindow();
            }
            if (GUILayout.Button("New Patrol Area"))
            {
                ShowPatrolWindow();
            }
            GUILayout.EndHorizontal();
        }

        protected static void ShowPatrolWindow()
        {
            GetWindow<PatrolAreaEditor>("Patrol Area Creator");
        }

        protected static void ShowRestWindow()
        {
            GetWindow<RestAreaEditor>("Rest Area Creator");
        }
    }
}
