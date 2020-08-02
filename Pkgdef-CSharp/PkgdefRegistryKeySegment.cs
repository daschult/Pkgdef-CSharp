using System.Collections.Generic;
using System.Linq;

namespace Pkgdef_CSharp
{
    internal class PkgdefRegistryKeySegment : PkgdefSegment
    {
        private readonly IReadOnlyList<PkgdefSegment> segments;

        public PkgdefRegistryKeySegment(IReadOnlyList<PkgdefSegment> segments)
        {
            PreCondition.AssertNotNullAndNotEmpty(segments, nameof(segments));
            PreCondition.AssertEqual(segments[0].GetSegmentType(), PkgdefSegmentType.RegistryKeyPath, "segments[0].GetSegmentType()");

            this.segments = segments;
        }

        public PkgdefRegistryKeyPathSegment GetRegistryKeyPath()
        {
            return (PkgdefRegistryKeyPathSegment)this.segments[0];
        }

        /// <inheritdoc/>
        public override int GetLength()
        {
            return this.segments.Last().GetAfterEndIndex() - this.segments.First().GetStartIndex();
        }

        /// <inheritdoc/>
        public override PkgdefSegmentType GetSegmentType()
        {
            return PkgdefSegmentType.RegistryKey;
        }

        /// <inheritdoc/>
        public override int GetStartIndex()
        {
            return this.segments.First().GetStartIndex();
        }

        /// <inheritdoc/>
        public override int GetAfterEndIndex()
        {
            return this.segments.Last().GetAfterEndIndex();
        }

        /// <inheritdoc/>
        public override int GetEndIndex()
        {
            return this.segments.Last().GetEndIndex();
        }

        /// <inheritdoc/>
        public override string GetText()
        {
            return string.Join("", this.segments.Select((PkgdefSegment segment) => segment.GetText()));
        }

        /// <inheritdoc/>
        public override bool Equals(object rhs)
        {
            return this.Equals(rhs as PkgdefRegistryKeySegment);
        }

        public bool Equals(PkgdefRegistryKeySegment rhs)
        {
            return rhs != null &&
                this.segments.SequenceEqual(rhs.segments);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int result = 0;
            foreach (PkgdefSegment segment in this.segments)
            {
                result ^= segment.GetHashCode();
            }
            return result;
        }
    }
}
