using System;
using UnityEngine;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class LabelTextAttribute : System.Attribute
    {
        public readonly string label;

        public LabelTextAttribute(string label)
        {
            this.label = label;
        }
    }
}