using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class PatrolAreaEditor : EditorWindow
    {
        // Area settings
        private string patrolAreaName = "New Patrol Area";
        private string waypointName = "Waypoint";
        private int waypointAmount = 5;

        // Waypoint settings
        private Vector2Int interactTime = new Vector2Int() { x = 2, y = 8 };
        private Vector2Int nodeSpread = new Vector2Int() { x = 1, y = 2 };
        private int waypointPriority = 0;

        // Area tooltips
        private GUIContent baseNameAreaTip = new GUIContent("Name in hierarchy", "What should the patrol area be named");
        private GUIContent amountTip = new GUIContent("Amount of nodes", "How many waypoint nodes should there be? You can always add or remove them later on");

        // Waypoint tooltips
        private GUIContent baseNamePointTip = new GUIContent("Name in hierarchy", "What should the waypoint be named");
        private GUIContent interactTip = new GUIContent("Interaction time", "How long should the npc interact at this waypoint when it is interacting");
        private GUIContent spreadTip = new GUIContent("Node spread", "The actual position per npc will vary from the center + range in order to have random positions so that it feels more dynamic");
        private GUIContent priorityTip = new GUIContent("Node priority", "How much priority should a waypoint have on average");

        private GUIStyles customStyle = new();

        [MenuItem("Tools/RaafOritme/Smart NPCs/Create Patrol Area")]
        public static void ShowWindow()
        {
            GetWindow<PatrolAreaEditor>("Patrol Area Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Patrol Area", EditorStyles.boldLabel);
            patrolAreaName = EditorGUILayout.TextField(baseNameAreaTip, patrolAreaName);
            waypointAmount = EditorGUILayout.IntField(amountTip, waypointAmount);
            
            customStyle.DrawHorizontalGUILine();
            GUILayout.Label("Average waypoint", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("After generating the area you can fine tune every single waypoint individually in the inspector.", MessageType.Info);
            waypointName = EditorGUILayout.TextField(baseNamePointTip, waypointName);
            waypointPriority = EditorGUILayout.IntField(priorityTip, waypointPriority);
            interactTime = EditorGUILayout.Vector2IntField(interactTip, interactTime);
            nodeSpread = EditorGUILayout.Vector2IntField(spreadTip, nodeSpread);

            customStyle.DrawHorizontalGUILine();
            if (GUILayout.Button("Generate Patrol Area"))
            {
                GeneratePatrolArea();
            }
        }

        protected void GeneratePatrolArea()
        {
            GameObject newAreaObject = new GameObject();
            newAreaObject.name = patrolAreaName;
            PatrolArea newArea = newAreaObject.AddComponent<PatrolArea>();
            Selection.activeGameObject = newAreaObject;

            // Used to spawn nodes in a circular motion
            GameObject tempObj = new GameObject();

            for (int i = 0; i < waypointAmount; i++)
            {
                tempObj.transform.eulerAngles = new Vector3(0, 360f / waypointAmount * i, 0);

                GameObject newWaypointObject = new GameObject();
                PatrolNode waypoint = newWaypointObject.AddComponent<PatrolNode>();
                waypoint.name = $"{waypointName} ({i + 1})";
                waypoint.node.interactTime = interactTime;
                waypoint.node.nodeSpread = nodeSpread;
                waypoint.node.priority = waypointPriority;

                newArea.nodes.Add(waypoint);

                newWaypointObject.transform.parent = tempObj.transform;
                newWaypointObject.transform.localPosition += Vector3.forward * 3;
                newWaypointObject.transform.parent = newAreaObject.transform;
            }

            DestroyImmediate(tempObj);
        }
    }
}
