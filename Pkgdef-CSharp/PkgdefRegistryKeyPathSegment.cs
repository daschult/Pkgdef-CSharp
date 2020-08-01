using System.Collections.Generic;
using System.Linq;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// A PkgdefSegment that contains a path to a Registry Key.
    /// </summary>
    internal class PkgdefRegistryKeyPathSegment : PkgdefSegment
    {
        private readonly IReadOnlyList<PkgdefToken> tokens;

        public PkgdefRegistryKeyPathSegment(IReadOnlyList<PkgdefToken> tokens)
        {
            PreCondition.AssertNotNullAndNotEmpty(tokens, nameof(tokens));
            PreCondition.AssertEqual(tokens[0].GetTokenType(), PkgdefTokenType.LeftSquareBracket, "tokens[0].GetTokenType()");

            this.tokens = tokens;
        }

        public PkgdefToken GetLeftSquareBracket()
        {
            return this.tokens[0];
        }

        public PkgdefToken GetRightSquareBracket()
        {
            PkgdefToken result = this.tokens.Last();
            if (result.GetTokenType() != PkgdefTokenType.RightSquareBracket)
            {
                result = null;
            }
            return result;
        }

        /// <inheritdoc/>
        public override int GetLength()
        {
            return this.tokens.Last().GetAfterEndIndex() - this.tokens.First().GetStartIndex();
        }

        /// <inheritdoc/>
        public override PkgdefSegmentType GetSegmentType()
        {
            return PkgdefSegmentType.RegistryKeyPath;
        }

        /// <inheritdoc/>
        public override int GetStartIndex()
        {
            return this.tokens.First().GetStartIndex();
        }

        /// <inheritdoc/>
        public override int GetAfterEndIndex()
        {
            return this.tokens.Last().GetAfterEndIndex();
        }

        /// <inheritdoc/>
        public override int GetEndIndex()
        {
            return this.tokens.Last().GetEndIndex();
        }

        /// <inheritdoc/>
        public override string GetText()
        {
            return string.Join("", this.tokens.Select((PkgdefToken token) => token.GetText()));
        }

        /// <inheritdoc/>
        public override bool Equals(object rhs)
        {
            return this.Equals(rhs as PkgdefRegistryKeyPathSegment);
        }

        public bool Equals(PkgdefRegistryKeyPathSegment rhs)
        {
            return rhs != null &&
                this.tokens.SequenceEqual(rhs.tokens);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int result = 0;
            foreach (PkgdefToken token in this.tokens)
            {
                result ^= token.GetHashCode();
            }
            return result;
        }
    }
}
