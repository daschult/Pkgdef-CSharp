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

        public static PkgdefToken ForwardSlash(int startIndex)
        {
            return new PkgdefToken(startIndex, '/', PkgdefTokenType.ForwardSlash);
        }

        public static PkgdefToken Backslash(int startIndex)
        {
            return new PkgdefToken(startIndex, '\\', PkgdefTokenType.Backslash);
        }

        public static PkgdefToken Whitespace(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.Whitespace);
        }

        public static PkgdefToken NewLine(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.NewLine);
        }

        public static PkgdefToken AtSign(int startIndex)
        {
            return new PkgdefToken(startIndex, "@", PkgdefTokenType.AtSign);
        }

        public static PkgdefToken EqualsSign(int startIndex)
        {
            return new PkgdefToken(startIndex, "=", PkgdefTokenType.EqualsSign);
        }

        public static PkgdefToken DollarSign(int startIndex)
        {
            return new PkgdefToken(startIndex, "$", PkgdefTokenType.DollarSign);
        }

        public static PkgdefToken LeftSquareBracket(int startIndex)
        {
            return new PkgdefToken(startIndex, "[", PkgdefTokenType.LeftSquareBracket);
        }

        public static PkgdefToken RightSquareBracket(int startIndex)
        {
            return new PkgdefToken(startIndex, "]", PkgdefTokenType.RightSquareBracket);
        }

        public static PkgdefToken DoubleQuote(int startIndex)
        {
            return new PkgdefToken(startIndex, "\"", PkgdefTokenType.DoubleQuote);
        }

        public static PkgdefToken Colon(int startIndex)
        {
            return new PkgdefToken(startIndex, ":", PkgdefTokenType.Colon);
        }

        public static PkgdefToken LeftCurlyBracket(int startIndex)
        {
            return new PkgdefToken(startIndex, '{', PkgdefTokenType.LeftCurlyBracket);
        }

        public static PkgdefToken RightCurlyBracket(int startIndex)
        {
            return new PkgdefToken(startIndex, '}', PkgdefTokenType.RightCurlyBracket);
        }

        public static PkgdefToken Dash(int startIndex)
        {
            return new PkgdefToken(startIndex, '-', PkgdefTokenType.Dash);
        }

        public static PkgdefToken Letters(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.Letters);
        }

        public static PkgdefToken Digits(int startIndex, string text)
        {
            return new PkgdefToken(startIndex, text, PkgdefTokenType.Digits);
        }

        public static PkgdefToken Unrecognized(int startIndex, char character)
        {
            return new PkgdefToken(startIndex, character, PkgdefTokenType.Unrecognized);
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
