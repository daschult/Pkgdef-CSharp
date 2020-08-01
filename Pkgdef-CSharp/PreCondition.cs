using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Assert that the provided value is not null and not empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="valueName">The name of the value to check.</param>
        public static void AssertNotNullAndNotEmpty<T>(IEnumerable<T> value, string valueName)
        {
            PreCondition.AssertNotNull(value, valueName);
            if (!value.Any())
            {
                throw new PreConditionException($"{valueName} cannot be empty.");
            }
        }

        /// <summary>
        /// Assert that the provided value is equal to the provided expected value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="expectedValue">The expected value that the value should be equal to.</param>
        /// <param name="valueName">The name of the value to check.</param>
        public static void AssertEqual(char value, char expectedValue, string valueName)
        {
            if (value != expectedValue)
            {
                throw new PreConditionException($"{valueName} ('{value}') must be equal to '{expectedValue}'.");
            }
        }

        /// <summary>
        /// Assert that the provided value is equal to the provided expected value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="expectedValue">The expected value that the value should be equal to.</param>
        /// <param name="valueName">The name of the value to check.</param>
        public static void AssertEqual<T>(T value, T expectedValue, string valueName)
        {
            if (!object.Equals(value, expectedValue))
            {
                throw new PreConditionException($"{valueName} ({value}) must be equal to {expectedValue}.");
            }
        }

        /// <summary>
        /// Assert that the provided value is greater than or equal to the provided lowerBound.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="lowerBound">The lower bound that the value must be greater than or equal
        /// to.</param>
        /// <param name="valueName">The name of the value to check.</param>
        public static void AssertGreaterThanOrEqualTo(int value, int lowerBound, string valueName)
        {
            if (value < lowerBound)
            {
                throw new PreConditionException($"{valueName} ({value}) must be greater than or equal to {lowerBound}.");
            }
        }
    }
}
