using System;
using System.Diagnostics;

namespace z.SuperLambda
{
    /// <summary>
    /// Parse UnfindType Exception
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    public class ParseUnfindTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseUnfindTypeException"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="errorIndex">Index of the error.</param>
        public ParseUnfindTypeException(string typeName, int errorIndex)
            : base($"在{errorIndex}附近没有找到类型{typeName}")
        {
        }
    }
}
