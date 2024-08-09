using System;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelWidthAttribute : System.Attribute
    {
        public readonly float width;

        public LabelWidthAttribute(float width)
        {
            this.width = width;
        }
    }
}