using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class NPCGenerator : EditorWindow
    {
        private enum WindowType
        {
            Standard,
            Sample
        }

        private WindowType currentWindow = WindowType.Standard;

        private StandardNPCGenerator standardNpcLogic;
        private GUIStyles customStyles = new();

        private Vector2 scrollPosition;

        [MenuItem("Tools/RaafOritme/Smart NPCs/NPC Generator %t")]
        public static void ShowWindow()
        {
            GetWindow<NPCGenerator>("NPC Generator", true);
        }
        
        // TIP: This tool can contain as many as various windows as you want, you can expand this in here, see the commented sample below.
        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

            GUILayout.BeginHorizontal();
            DrawTabButton("Standard NPC", WindowType.Standard);
            //DrawTabButton("Sample NPC", WindowType.Sample);
            GUILayout.EndHorizontal();

            switch (currentWindow)
            {
                default:
                case WindowType.Standard:
                    DrawWindowStandard();
                    break;
                //case WindowType.Sample:
                //    DrawWindowSample();
                //    break;
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawTabButton(string label, WindowType windowType)
        {
            GUIStyle style = currentWindow == windowType ? customStyles.selectedStyle : customStyles.unselectedStyle;
            if (GUILayout.Button(label, style))
            {
                currentWindow = windowType;
            }
        }

        private void DrawWindowStandard()
        {
            if (standardNpcLogic == null)
            {
                standardNpcLogic = new();
            }
            standardNpcLogic.GUIDraw();
        }
    }
}
