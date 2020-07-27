using System;
using System.Collections;
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
            return new Iterator<T>(enumerator);
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
    }

    /// <summary>
    /// A wrapper around an IEnumerator that adds HasStarted and HasCurrent properties.
    /// </summary>
    /// <typeparam name="T">The type of value that is contained by this Iterator.</typeparam>
    internal class Iterator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        /// <summary>
        /// Create a new Iterator that will iterate over the values in the provided IEnumerator.
        /// </summary>
        /// <param name="enumerator">The IEnumerator to iterate over.</param>
        internal Iterator(IEnumerator<T> enumerator)
        {
            PreCondition.AssertNotNull(enumerator, nameof(enumerator));

            this.enumerator = enumerator;
            try
            {
                this.HasCurrent = enumerator.Current != null;
                this.HasStarted = true;
            }
            catch (InvalidOperationException e)
            {
                // This exception is thrown when the IEnumerator hasn't started yet.
                this.HasCurrent = false;
                if (e.Message == "Enumeration has not started. Call MoveNext.")
                {
                    this.HasStarted = false;
                }
                else
                {
                    this.HasStarted = true;
                }
            }
            
        }

        /// <summary>
        /// Whether or not this Iterator has begun iterating over its values.
        /// </summary>
        public bool HasStarted { get; private set; }

        /// <inheritdoc/>
        public T Current
        {
            get
            {
                PreCondition.AssertTrue(this.HasCurrent, nameof(this.HasCurrent));

                return this.enumerator.Current;
            }
        }

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Whether or not this Iterator has a current value.
        /// </summary>
        public bool HasCurrent { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.enumerator.Dispose();
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            this.HasStarted = true;
            this.HasCurrent = this.enumerator.MoveNext();
            return this.HasCurrent;
        }

        /// <summary>
        /// Move to the next value in this Iterator.
        /// </summary>
        /// <returns>Whether or not a next value was found.</returns>
        public bool Next()
        {
            return this.MoveNext();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            this.enumerator.Reset();
        }
    }
}
