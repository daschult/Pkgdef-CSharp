namespace Pkgdef_CSharp
{
    /// <summary>
    /// A collection of assertions that can be used to validate both argument values and program
    /// state before a function is run.
    /// </summary>
    internal static class PreCondition
    {
        /// <summary>
        /// Assert that the provided value is true.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="valueName">The name of the value to check.</param>
        public static void AssertTrue(bool value, string valueName)
        {
            if (value != true)
            {
                throw new PreConditionException($"{valueName} cannot be false.");
            }
        }

        /// <summary>
        /// Assert that the provided value is not null.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="valueName">The name of the value to check.</param>
        public static void AssertNotNull(object value, string valueName)
        {
            if (value == null)
            {
                throw new PreConditionException($"{valueName} cannot be null.");
            }
        }
    }
}
