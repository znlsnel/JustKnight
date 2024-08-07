using System;
using UnityEngine;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class BlockquoteAttribute : System.Attribute
    {
        public readonly string text;

        public BlockquoteAttribute(string text)
        {
            this.text = text;
        }
    }
}