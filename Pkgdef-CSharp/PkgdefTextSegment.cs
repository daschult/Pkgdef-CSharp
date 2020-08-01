namespace Pkgdef_CSharp
{
    internal class PkgdefTextSegment : PkgdefSegment
    {
        private readonly int startIndex;
        private readonly string text;
        private readonly PkgdefSegmentType segmentType;

        private PkgdefTextSegment(int startIndex, string text, PkgdefSegmentType segmentType)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertNotNullAndNotEmpty(text, nameof(text));

            this.startIndex = startIndex;
            this.text = text;
            this.segmentType = segmentType;
        }

        public static PkgdefTextSegment Create(int startIndex, string text, PkgdefSegmentType segmentType)
        {
            return new PkgdefTextSegment(startIndex, text, segmentType);
        }

        /// <inheritdoc/>
        public override int GetStartIndex()
        {
            return this.startIndex;
        }

        /// <inheritdoc/>
        public override int GetLength()
        {
            return this.text.Length;
        }

        /// <inheritdoc/>
        public override string GetText()
        {
            return this.text;
        }

        /// <inheritdoc/>
        public override PkgdefSegmentType GetSegmentType()
        {
            return this.segmentType;
        }

        public override bool Equals(object rhs)
        {
            return this.Equals(rhs as PkgdefTextSegment);
        }

        public bool Equals(PkgdefTextSegment rhs)
        {
            return rhs != null &&
                this.startIndex == rhs.startIndex &&
                this.text == rhs.text &&
                this.segmentType == rhs.segmentType;
        }
    }
}
