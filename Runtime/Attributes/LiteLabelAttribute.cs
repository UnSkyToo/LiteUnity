using System;
using System.Diagnostics;

namespace LiteUnity.Runtime.Attributes
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class LiteLabelAttribute : Attribute
    {
        public string Label { get; }
    
        public LiteLabelAttribute(string label)
        {
            Label = label;
        }
    }
}