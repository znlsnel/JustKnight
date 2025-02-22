using System;
using UnityEngine;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : System.Attribute
    {
        public readonly string message = null;
        public RequiredAttribute() { }
        public RequiredAttribute(string message)
        {
            this.message = message;
        }
    }
}