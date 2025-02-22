using System;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : System.Attribute
    {
        public readonly string label = null;
        public readonly InspectorButtonSize size = InspectorButtonSize.Small;

        public ButtonAttribute() { }

        public ButtonAttribute(string label)
        {
            this.label = label;
        }

        public ButtonAttribute(InspectorButtonSize size)
        {
            this.size = size;
        }

        public ButtonAttribute(string label, InspectorButtonSize size)
        {
            this.label = label;
            this.size = size;
        }
    }
}