using System;
using System.Collections.Generic;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// A wrapper around an IEnumerator that adds HasStarted and HasCurrent properties.
    /// </summary>
    /// <typeparam name="T">The type of value that is contained by this Iterator.</typeparam>
    internal class EnumeratorIterator<T> : IteratorBase<T>
    {
        private readonly IEnumerator<T> enumerator;
        private bool hasStarted;
        private bool hasCurrent;

        /// <summary>
        /// Create a new Iterator that will iterate over the values in the provided IEnumerator.
        /// </summary>
        /// <param name="enumerator">The IEnumerator to iterate over.</param>
        internal EnumeratorIterator(IEnumerator<T> enumerator)
        {
            PreCondition.AssertNotNull(enumerator, nameof(enumerator));

            this.enumerator = enumerator;
            try
            {
                this.hasCurrent = enumerator.Current != null;
                this.hasStarted = true;
            }
            catch (InvalidOperationException e)
            {
                // This exception is thrown when the IEnumerator hasn't started yet.
                this.hasCurrent = false;
                if (e.Message == "Enumeration has not started. Call MoveNext.")
                {
                    this.hasStarted = false;
                }
                else
                {
                    this.hasStarted = true;
                }
            }
        }

        /// <inheritdoc/>
        public override bool HasStarted()
        {
            return this.hasStarted;
        }

        /// <inheritdoc/>
        public override T Current
        {
            get
            {
                PreCondition.AssertTrue(this.HasCurrent(), "this.HasCurrent()");

                return this.enumerator.Current;
            }
        }

        /// <inheritdoc/>
        public override bool HasCurrent()
        {
            return this.hasCurrent;
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            this.enumerator.Dispose();
        }

        /// <inheritdoc/>
        public override bool MoveNext()
        {
            this.hasStarted = true;
            this.hasCurrent = this.enumerator.MoveNext();
            return this.hasCurrent;
        }

        /// <inheritdoc/>
        public override void Reset()
        {
            this.enumerator.Reset();
        }
    }
}
