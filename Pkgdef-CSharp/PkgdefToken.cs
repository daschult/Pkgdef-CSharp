namespace Pkgdef_CSharp
{
    /// <summary>
    /// A token within a PKGDEF document.
    /// </summary>
    internal class PkgdefToken
    {
        private readonly int startIndex;
        private readonly string text;
        private readonly PkgdefTokenType tokenType;

        public PkgdefToken(int startIndex, char character, PkgdefTokenType tokenType)
            : this(startIndex, character.ToString(), tokenType)
        {
        }

        public PkgdefToken(int startIndex, string text, PkgdefTokenType tokenType)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertNotNullAndNotEmpty(text, nameof(text));

            this.startIndex = startIndex;
            this.text = text;
            this.tokenType = tokenType;
        }

        public static PkgdefToken LineComment(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.LineComment);
        }

        public static PkgdefToken Whitespace(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.Whitespace);
        }

        public static PkgdefToken QuotedString(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.QuotedString);
        }

        public static PkgdefToken AtSign(int startIndex)
        {
            return new PkgdefToken(startIndex, "@", PkgdefTokenType.AtSign);
        }

        public static PkgdefToken Unrecognized(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.Unrecognized);
        }

        /// <summary>
        /// Get the start index in the document where the PkgdefToken begins.
        /// </summary>
        public int GetStartIndex()
        {
            return this.startIndex;
        }

        /// <summary>
        /// Get the number of characters that are contained by this PkgdefToken.
        /// </summary>
        public int GetLength()
        {
            return this.text.Length;
        }

        /// <summary>
        /// Get the last index that is contained by this PkgdefToken.
        /// </summary>
        public int GetEndIndex()
        {
            PreCondition.AssertGreaterThanOrEqualTo(this.GetLength(), 1, "this.GetLength()");

            return this.GetAfterEndIndex() - 1;
        }

        /// <summary>
        /// Get the index directly after (not contained by) this PkgdefToken.
        /// </summary>
        public int GetAfterEndIndex()
        {
            PreCondition.AssertGreaterThanOrEqualTo(this.GetLength(), 1, "this.GetLength()");

            return this.GetStartIndex() + this.GetLength();
        }

        /// <summary>
        /// Get the text of this PkgdefToken.
        /// </summary>
        public string GetText()
        {
            return this.text;
        }

        /// <summary>
        /// Get the token type of this PkgdefToken.
        /// </summary>
        /// <returns>The token type of this PkgdefToken.</returns>
        public PkgdefTokenType GetTokenType()
        {
            return this.tokenType;
        }

        /// <inheritdoc/>
        public override bool Equals(object rhs)
        {
            return this.Equals(rhs as PkgdefToken);
        }

        public bool Equals(PkgdefToken rhs)
        {
            return rhs != null &&
                this.startIndex == rhs.startIndex &&
                this.text == rhs.text &&
                this.tokenType == rhs.tokenType;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.startIndex.GetHashCode() ^
                this.text.GetHashCode() ^
                this.tokenType.GetHashCode();
        }
    }
}
