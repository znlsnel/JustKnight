using System;
using UnityEngine;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class DisableIfAttribute : System.Attribute
    {
        public readonly string condition;

        public DisableIfAttribute(string condition)
        {
            this.condition = condition;
        }
    }
}