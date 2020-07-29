namespace Pkgdef_CSharp
{
    /// <summary>
    /// A segment within a PKGDEF document.
    /// </summary>
    internal class PkgdefSegment
    {
        private readonly int startIndex;
        private readonly string text;
        private readonly PkgdefSegmentType segmentType;

        public PkgdefSegment(int startIndex, string text, PkgdefSegmentType segmentType)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertNotNullAndNotEmpty(text, nameof(text));
            
            this.startIndex = startIndex;
            this.text = text;
            this.segmentType = segmentType;
        }

        /// <summary>
        /// Get the start index in the document where the PkgdefSegment begins.
        /// </summary>
        public int GetStartIndex()
        {
            return this.startIndex;
        }

        /// <summary>
        /// Get the number of characters that are contained by this PkgdefSegment.
        /// </summary>
        public int GetLength()
        {
            return this.text.Length;
        }

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
        public string GetText()
        {
            return this.text;
        }

        /// <summary>
        /// Get the segment type of this PkgdefSegment.
        /// </summary>
        /// <returns>The segment type of this PkgdefSegment.</returns>
        public PkgdefSegmentType GetSegmentType()
        {
            return this.segmentType;
        }
    }
}
