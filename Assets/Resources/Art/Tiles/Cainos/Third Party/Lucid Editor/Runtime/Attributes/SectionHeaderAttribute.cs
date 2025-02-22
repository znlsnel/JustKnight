using System;
using UnityEngine;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class SectionHeaderAttribute : System.Attribute
    {
        public readonly string title;

        public SectionHeaderAttribute(string title)
        {
            this.title = title;
        }
    }
}