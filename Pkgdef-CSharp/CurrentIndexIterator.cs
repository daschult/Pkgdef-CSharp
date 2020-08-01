using System.Collections.Generic;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// Static helper methods for the generic Iterator type.
    /// </summary>
    internal static class CurrentIndexIterator
    {
        /// <summary>
        /// Create a new CurrentIndexIterator that will iterate over the values in the provided IEnumerable.
        /// </summary>
        /// <param name="iterator">The Iterator to iterate over.</param>
        /// <returns>A new CurrentIndexIterator that will iterate over the values in the provided IEnumerable.</returns>
        public static CurrentIndexIterator<T> Create<T>(Iterator<T> iterator)
        {
            return new CurrentIndexIterator<T>(iterator);
        }

        /// <summary>
        /// Create a new CurrentIndexIterator that will iterate over the values in the provided IEnumerator.
        /// </summary>
        /// <param name="enumerator">The IEnumerator to iterate over.</param>
        /// <returns>A new CurrentIndexIterator that will iterate over the values in the provided IEnumerator.</returns>
        public static CurrentIndexIterator<T> Create<T>(IEnumerator<T> enumerator)
        {
            return CurrentIndexIterator.Create(Iterator.Create(enumerator));
        }

        /// <summary>
        /// Create a new CurrentIndexIterator that will iterate over the values in the provided IEnumerable.
        /// </summary>
        /// <param name="enumerable">The IEnumerable to iterate over.</param>
        /// <returns>A new CurrentIndexIterator that will iterate over the values in the provided IEnumerable.</returns>
        public static CurrentIndexIterator<T> Create<T>(IEnumerable<T> enumerable)
        {
            PreCondition.AssertNotNull(enumerable, nameof(enumerable));

            return CurrentIndexIterator.Create(Iterator.Create(enumerable));
        }
    }

    /// <summary>
    /// An Iterator that keeps track of how many values it has iterated over.
    /// </summary>
    internal class CurrentIndexIterator<T> : IteratorBase<T>
    {
        private readonly Iterator<T> iterator;
        private int currentIndex;

        internal CurrentIndexIterator(Iterator<T> iterator)
        {
            PreCondition.AssertNotNull(iterator, nameof(iterator));

            this.iterator = iterator;
            this.currentIndex = 0;
        }

        /// <inheritdoc/>
        public override T Current
        {
            get { return this.iterator.GetCurrent(); }
        }

        /// <summary>
        /// Get the index of the current value within the context of the values that are being
        /// iterated over.
        /// </summary>
        /// <returns>The index of the current value within the context of the values that are being
        /// iterated over.</returns>
        public int GetCurrentIndex()
        {
            PreCondition.AssertTrue(this.HasCurrent(), "this.HasCurrent()");

            return this.currentIndex;
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            this.iterator.Dispose();
        }

        /// <inheritdoc/>
        public override bool HasCurrent()
        {
            return this.iterator.HasCurrent();
        }

        /// <inheritdoc/>
        public override bool HasStarted()
        {
            return this.iterator.HasStarted();
        }

        /// <inheritdoc/>
        public override bool MoveNext()
        {
            bool hasStarted = this.HasStarted();
            bool result = this.iterator.Next();
            if (hasStarted && result)
            {
                this.currentIndex++;
            }
            return result;
        }
    }
}
