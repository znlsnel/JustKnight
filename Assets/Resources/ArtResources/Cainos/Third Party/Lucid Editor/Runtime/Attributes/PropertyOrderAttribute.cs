using System;
using UnityEngine;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class PropertyOrderAttribute : System.Attribute
    {
        public readonly int propertyOrder;

        public PropertyOrderAttribute(int propertyOrder)
        {
            this.propertyOrder = propertyOrder;
        }
    }
}