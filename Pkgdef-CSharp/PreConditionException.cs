using System;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// An exception that is thrown when a PreCondition assertion fails.
    /// </summary>
    internal class PreConditionException : Exception
    {
        /// <summary>
        /// Create a new PreConditionException with the provided message.
        /// </summary>
        /// <param name="message">The message that explains what the pre-condition failure was.</param>
        public PreConditionException(string message)
            : base(message)
        {
        }
    }
}
