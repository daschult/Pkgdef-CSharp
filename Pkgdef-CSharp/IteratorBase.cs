using System;
using System.Collections;
using System.Collections.Generic;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// A base abstract implementation of Iterator that contains common implementations to some of
    /// Iterator's methods.
    /// </summary>
    /// <typeparam name="T">The type of values that this Iterator will iterate over.</typeparam>
    internal abstract class IteratorBase<T> : Iterator<T>
    {
        /// <inheritdoc/>
        public abstract T Current { get; }

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <inheritdoc/>
        public abstract void Dispose();

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public abstract bool HasCurrent();

        /// <inheritdoc/>
        public abstract bool HasStarted();

        /// <inheritdoc/>
        public abstract bool MoveNext();

        /// <inheritdoc/>
        public virtual void Reset()
        {
            throw new NotSupportedException();
        }
    }
}
