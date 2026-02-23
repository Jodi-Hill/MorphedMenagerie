using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class RestAreaEditor : EditorWindow
    {
        // Area settings
        private string restAreaName = "New Rest Area";
        private string seatName = "Seat";
        private int seatAmount = 3;
        private LayerMask restLayer;

        // Seat settings
        private AnimationType seatAction = AnimationType.SITTING;

        // Area tooltips
        private GUIContent baseNameAreaTip = new GUIContent("Name in hierarchy", "What should the rest area be named");
        private GUIContent amountTip = new GUIContent("Amount of seats", "How many seats should there be");
        private GUIContent restLayerTip = new GUIContent("LayerMask of rest areas", "In which LayerMask can rest areas be detected? Make sure to isolate this mask in the physics settings");

        // Waypoint tooltips
        private GUIContent baseNameSeatTip = new GUIContent("Name in hierarchy", "What should the seat be named");
        private GUIContent actionTip = new GUIContent("Seat action", "What should the npc perform while being seated");
        
        private GUIStyles customStyle = new();

        [MenuItem("Tools/RaafOritme/Smart NPCs/Create Rest Area")]
        public static void ShowWindow()
        {
            GetWindow<RestAreaEditor>("Rest Area Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Rest Area", EditorStyles.boldLabel);
            restAreaName = EditorGUILayout.TextField(baseNameAreaTip, restAreaName);
            seatAmount = EditorGUILayout.IntField(amountTip, seatAmount);
            restLayer = EditorGUILayout.LayerField(restLayerTip, restLayer);

            customStyle.DrawHorizontalGUILine();
            GUILayout.Label("Average seat", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("After generating the area you can fine tune every single seat individually in the inspector.", MessageType.Info);
            seatName = EditorGUILayout.TextField(baseNameSeatTip, seatName);
            seatAction = (AnimationType)EditorGUILayout.EnumPopup(actionTip, seatAction);

            customStyle.DrawHorizontalGUILine();
            if (GUILayout.Button("Generate Rest Area"))
            {
                GenerateRestArea();
            }
        }

        protected void GenerateRestArea()
        {
            GameObject newAreaObject = new GameObject();
            newAreaObject.AddComponent<BoxCollider>();
            newAreaObject.name = restAreaName;
            newAreaObject.layer = restLayer;

            RestingArea newArea = newAreaObject.AddComponent<RestingArea>();
            newArea.restingInfo = new();
            newArea.restingInfo.restingSpots = new();
            newArea.restingInfo.seatAvailable = new();

            GameObject newEntranceObject = new GameObject();
            newEntranceObject.name = "Entrance";
            newEntranceObject.transform.parent = newAreaObject.transform;
            newEntranceObject.transform.localPosition = Vector3.forward * 3;
            newArea.restingInfo.entrance = newEntranceObject.transform;
            Selection.activeGameObject = newAreaObject;

            for (int i = 0; i < seatAmount; i++)
            {
                GameObject newSeat = new GameObject();
                RestingObject seatRest = newSeat.AddComponent<RestingObject>();
                seatRest.name = $"{seatName} ({i + 1})";
                seatRest.animationType = seatAction;
                newSeat.transform.parent = newAreaObject.transform;
                newSeat.transform.localPosition = new Vector3((-seatAmount / 2f) + i * 2, 0, 0);

                newArea.restingInfo.restingSpots.Add(newSeat.transform);
                newArea.restingInfo.availableSeats++;
                newArea.restingInfo.seatAvailable.Add(false);
            }
        }
    }
}
