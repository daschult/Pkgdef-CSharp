using System.Collections.Generic;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// Static helper methods for the generic Iterator type.
    /// </summary>
    internal static class Iterator
    {
        /// <summary>
        /// Create a new Iterator that will iterate over the values in the provided IEnumerator.
        /// </summary>
        /// <param name="enumerator">The IEnumerator to iterate over.</param>
        /// <returns>A new Iterator that will iterate over the values in the provided IEnumerator.</returns>
        public static Iterator<T> Create<T>(IEnumerator<T> enumerator)
        {
            return new EnumeratorIterator<T>(enumerator);
        }

        /// <summary>
        /// Create a new Iterator that will iterate over the values in the provided IEnumerable.
        /// </summary>
        /// <param name="enumerable">The IEnumerable to iterate over.</param>
        /// <returns>A new Iterator that will iterate over the values in the provided IEnumerable.</returns>
        public static Iterator<T> Create<T>(IEnumerable<T> enumerable)
        {
            PreCondition.AssertNotNull(enumerable, nameof(enumerable));

            return Iterator.Create(enumerable.GetEnumerator());
        }

        /// <summary>
        /// Ensure that the Iterator has started.
        /// </summary>
        public static void EnsureHasStarted<T>(this Iterator<T> iterator)
        {
            PreCondition.AssertNotNull(iterator, nameof(iterator));

            if (!iterator.HasStarted())
            {
                iterator.Next();
            }
        }

        /// <summary>
        /// Move to the next value in this Iterator.
        /// </summary>
        /// <returns>Whether or not a next value was found.</returns>
        public static bool Next<T>(this Iterator<T> iterator)
        {
            PreCondition.AssertNotNull(iterator, nameof(iterator));

            return iterator.MoveNext();
        }

        /// <summary>
        /// Get the current value in this iterator.
        /// </summary>
        /// <typeparam name="T">The type of values iterate over by the iterator.</typeparam>
        /// <param name="iterator">The iterator to get the current value of.</param>
        /// <returns>The current value of this iterator.</returns>
        public static T GetCurrent<T>(this Iterator<T> iterator)
        {
            PreCondition.AssertNotNull(iterator, nameof(iterator));

            return iterator.Current;
        }

        /// <summary>
        /// Take and return the current value and advance this iterator the next value.
        /// </summary>
        /// <returns>The taken current value.</returns>
        public static T TakeCurrent<T>(this Iterator<T> iterator)
        {
            PreCondition.AssertNotNull(iterator, nameof(iterator));
            PreCondition.AssertTrue(iterator.HasCurrent(), "iterator.HasCurrent()");

            T result = iterator.Current;
            iterator.Next();

            return result;
        }
    }

    /// <summary>
    /// A wrapper around an IEnumerator that adds HasStarted() and HasCurrent() functions.
    /// </summary>
    /// <typeparam name="T">The type of value that is contained by this Iterator.</typeparam>
    internal interface Iterator<T> : IEnumerator<T>, IEnumerable<T>
    {
        /// <summary>
        /// Whether or not this Iterator has begun iterating over its values.
        /// </summary>
        bool HasStarted();

        /// <summary>
        /// Whether or not this Iterator has a current value.
        /// </summary>
        bool HasCurrent();
    }
}
