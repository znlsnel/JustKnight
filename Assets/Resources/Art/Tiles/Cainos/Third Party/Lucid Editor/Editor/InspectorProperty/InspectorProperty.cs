using System;
using UnityEditor;

namespace Cainos.LucidEditor
{
    public abstract class InspectorProperty
    {
        public readonly SerializedObject serializedObject;
        public readonly SerializedProperty serializedProperty;
        public readonly object parentObject;
        public readonly string name;
        public readonly Type type;

        public readonly System.Attribute[] attributes;

        public TAttribute GetAttribute<TAttribute>() where TAttribute : System.Attribute
        {
            foreach (System.Attribute att in attributes)
            {
                if (att is TAttribute)
                {
                    return (TAttribute)att;
                }
            }
            return null;
        }

        public bool TryGetAttribute<TAttribute>(out TAttribute result) where TAttribute : System.Attribute
        {
            foreach (System.Attribute att in attributes)
            {
                if (att is TAttribute)
                {
                    result = (TAttribute)att;
                    return true;
                }
            }
            result = null;
            return false;
        }

        internal InspectorProperty(SerializedObject serializedObject, SerializedProperty serializedProperty, object parentObject, string name, System.Attribute[] attributes)
        {
            this.serializedObject = serializedObject;
            if (serializedProperty != null)
            {
                this.serializedProperty = serializedProperty.Copy();
                type = serializedProperty.GetUnderlyingType();
            }
            this.parentObject = parentObject;
            this.displayName = name;
            this.name = name;
            this.attributes = attributes;
        }

        public int order;
        public bool isHidden;
        public bool isEditable = true;
        public bool hideLabel;
        public int indent;
        public string displayName;
        public bool allowSceneObject = true;

        public bool isInGroup => _isInGroup;
        public bool changed => _changed;

        internal bool _changed;
        internal bool _isInGroup;
        internal abstract void Initialize();
        internal abstract void OnBeforeInspectorGUI();
        internal abstract void OnAfterInspectorGUI();
        internal abstract void Draw();

        internal virtual void Reset()
        {
            order = 0;
            isHidden = false;
            isEditable = true;
            hideLabel = false;
            indent = 0;
            displayName = string.Empty;
            allowSceneObject = true;
            _changed = false;
        }
    }

}