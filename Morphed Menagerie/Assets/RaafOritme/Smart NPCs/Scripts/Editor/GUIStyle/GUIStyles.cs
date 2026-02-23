using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    /// <summary>
    /// This work around prevents nullrefs from being thrown due to the GUIStyle not always being loaded on time
    /// </summary>
    public class GUIStyles
    {
        public GUIStyleWrapper selectedStyle = new GUIStyleWrapper(() =>
        {
            GUIStyle selectedTabStyle = new GUIStyle(EditorStyles.toolbarButton);
            selectedTabStyle.normal.background = TabBackground(1, 1, new Color(0.4f, 0.4f, 0.4f));

            GUIStyle unselectedTabStyle = new GUIStyle(EditorStyles.toolbarButton);

            return selectedTabStyle;
        });

        public GUIStyleWrapper unselectedStyle = new GUIStyleWrapper(() =>
        {
            GUIStyle unselectedTabStyle = new GUIStyle(EditorStyles.toolbarButton);

            return unselectedTabStyle;
        });

        /// <summary>
        /// Draw the background for a tab.
        /// </summary>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <param name="_color"></param>
        /// <returns></returns>
        public static Texture2D TabBackground(int _width, int _height, Color _color)
        {
            Color[] colors = new Color[_width * _height];
            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i] = _color;
            }
            Texture2D result = new Texture2D(_width, _height);
            result.SetPixels(colors);
            result.Apply();
            return result;
        }

        /// <summary>
        /// Draw a horizontal line in the GUI.
        /// </summary>
        /// <param name="_height"></param>
        public void DrawHorizontalGUILine(int _height = 1)
        {
            GUILayout.Space(12);

            Rect rect = GUILayoutUtility.GetRect(10, _height, GUILayout.ExpandWidth(true));
            rect.height = _height;
            rect.xMin = 0;
            rect.xMax = EditorGUIUtility.currentViewWidth;

            Color lineColor = new Color(0.10196f, 0.10196f, 0.10196f, 1);
            EditorGUI.DrawRect(rect, lineColor);
            GUILayout.Space(4);
        }
    }
}
