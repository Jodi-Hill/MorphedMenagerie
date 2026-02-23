using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class GUIStyleWrapper
    {
        private GUIStyle style = null;
        private System.Func<GUIStyle> initializer;

        public GUIStyleWrapper(System.Func<GUIStyle> _initializer)
        {
            initializer = _initializer;
        }

        /// <summary>
        /// Execute the initializer from the style.
        /// </summary>
        /// <param name="_style"></param>
        public static implicit operator GUIStyle(GUIStyleWrapper _style)
        {
            if (_style.style == null)
            {
                _style.style = _style.initializer();
                _style.initializer = null;
            }
            return _style.style;
        }

        /// <summary>
        /// Return this GUI Style.
        /// </summary>
        /// <returns></returns>
        public GUIStyle Style()
        {
            return this;
        }
    }
}
