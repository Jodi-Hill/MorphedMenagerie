using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class SceneToGoap : EditorWindow
    {
        [MenuItem("Tools/RaafOritme/Smart NPCs/GOAPify Scene")]
        public static void ShowWindow()
        {
            GetWindow<SceneToGoap>("Scene Upgrader (GOAP)");
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("If you are using a GOAP powered agent in the scene you will need to prepare the scene for this type of AI. " +
                "This tool will automatically all required components to all existing basic scene objects such as patrol areas. " +
                "This means that the GOAP powered agents can utulize these options as well.", MessageType.Info);

            if (GUILayout.Button("GOAPify Scene"))
            {
                Goapifier();
            }

            EditorGUILayout.HelpBox("You can easily remove all the added GOAP components through the button below.", MessageType.Info);
            if (GUILayout.Button("Remove GOAPify"))
            {
                UnGoapify();
            }
        }

        private void Goapifier()
        {
            BaseEnvironment[] components = Resources.FindObjectsOfTypeAll<BaseEnvironment>();

            foreach (BaseEnvironment component in components)
            {
                component.Goapify();
            }
        }

        private void UnGoapify()
        {
            BaseAction[] components = Resources.FindObjectsOfTypeAll<BaseAction>();

            foreach (BaseAction component in components)
            {
                DestroyImmediate(component);
            }
        }
    }
}
