using System;
using UnityEngine;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValidateInputAttribute : System.Attribute
    {
        public readonly string condition;
        public readonly string message;
        public readonly HelpBoxMessageType type = HelpBoxMessageType.Error;

        public ValidateInputAttribute(string condition)
        {
            this.condition = condition;
        }

        public ValidateInputAttribute(string condition, string message)
        {
            this.condition = condition;
            this.message = message;
        }

        public ValidateInputAttribute(string condition, string message, HelpBoxMessageType type)
        {
            this.condition = condition;
            this.message = message;
            this.type = type;
        }
    }
}