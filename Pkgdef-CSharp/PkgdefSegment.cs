using System.Collections.Generic;
using System.Text;

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
        public virtual int GetEndIndex()
        {
            PreCondition.AssertGreaterThanOrEqualTo(this.GetLength(), 1, "this.GetLength()");

            return this.GetAfterEndIndex() - 1;
        }

        /// <summary>
        /// Get the index directly after (not contained by) this PkgdefSegment.
        /// </summary>
        public virtual int GetAfterEndIndex()
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('{');

            builder.Append($"\"startIndex\":{this.GetStartIndex()}");
            
            builder.Append(',');
            builder.Append($"\"segmentType\":\"{this.GetSegmentType()}\"");

            builder.Append(',');
            builder.Append($"\"text\":\"{Strings.Escape(this.GetText())}\"");

            builder.Append('}');

            return builder.ToString();
        }

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

        public static PkgdefRegistryKeyPathSegment RegistryKeyPath(int startIndex, string text)
        {
            return PkgdefDocument.ParseRegistryKeyPath(startIndex, text);
        }

        public static PkgdefRegistryKeyDataItemSegment RegistryKeyDataItem(int startIndex, string text)
        {
            return PkgdefDocument.ParseRegistryKeyDataItem(startIndex, text);
        }
    }
}
