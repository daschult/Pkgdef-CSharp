using System.Collections.Generic;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// A segment within a PKGDEF document.
    /// </summary>
    internal abstract class PkgdefSegment
    {
        /// <summary>
        /// Get the start index in the document where the PkgdefSegment begins.
        /// </summary>
        public abstract int GetStartIndex();

        /// <summary>
        /// Get the number of characters that are contained by this PkgdefSegment.
        /// </summary>
        public abstract int GetLength();

        /// <summary>
        /// Get the last index that is contained by this PkgdefSegment.
        /// </summary>
        public int GetEndIndex()
        {
            PreCondition.AssertGreaterThanOrEqualTo(this.GetLength(), 1, "this.GetLength()");

            return this.GetAfterEndIndex() - 1;
        }

        /// <summary>
        /// Get the index directly after (not contained by) this PkgdefSegment.
        /// </summary>
        public int GetAfterEndIndex()
        {
            PreCondition.AssertGreaterThanOrEqualTo(this.GetLength(), 1, "this.GetLength()");

            return this.GetStartIndex() + this.GetLength();
        }

        /// <summary>
        /// Get the text of this PkgdefSegment.
        /// </summary>
        public abstract string GetText();

        /// <summary>
        /// Get the segment type of this PkgdefSegment.
        /// </summary>
        /// <returns>The segment type of this PkgdefSegment.</returns>
        public abstract PkgdefSegmentType GetSegmentType();

        public static PkgdefSegment Whitespace(int startIndex, string text)
        {
            return PkgdefTextSegment.Create(startIndex, text, PkgdefSegmentType.Whitespace);
        }

        public static PkgdefSegment NewLine(int startIndex, string text)
        {
            return PkgdefTextSegment.Create(startIndex, text, PkgdefSegmentType.NewLine);
        }

        public static PkgdefSegment Unrecognized(int startIndex, string text)
        {
            return PkgdefTextSegment.Create(startIndex, text, PkgdefSegmentType.Unrecognized);
        }

        public static PkgdefSegment LineComment(int startIndex, string text)
        {
            return PkgdefTextSegment.Create(startIndex, text, PkgdefSegmentType.LineComment);
        }
    }
}
