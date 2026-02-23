using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class SubclassPicker : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SubclassPicker))]
    public class SubclassPickerDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight(_property);
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            Type t = fieldInfo.FieldType;
            string typeName = _property.managedReferenceValue?.GetType().Name ?? "Not set";

            Rect dropdownRect = _position;
            dropdownRect.x += EditorGUIUtility.labelWidth + 2;
            dropdownRect.width -= EditorGUIUtility.labelWidth + 2;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;
            if (EditorGUI.DropdownButton(dropdownRect, new(typeName), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();

                // null
                menu.AddItem(new GUIContent("None"), _property.managedReferenceValue == null, () =>
                {
                    _property.managedReferenceValue = null;
                    _property.serializedObject.ApplyModifiedProperties();
                });

                // inherited types
                foreach (Type type in GetClasses(t))
                {
                    menu.AddItem(new GUIContent(type.Name), typeName == type.Name, () =>
                    {
                        _property.managedReferenceValue = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                        _property.serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            }
            EditorGUI.PropertyField(_position, _property, _label, true);
        }

        private IEnumerable GetClasses(Type _baseType)
        {
            return Assembly.GetAssembly(_baseType).GetTypes().Where(t => t.IsClass && !t.IsAbstract && _baseType.IsAssignableFrom(t));
        }
    }
#endif
}
