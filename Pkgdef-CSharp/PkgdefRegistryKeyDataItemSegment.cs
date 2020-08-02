using System.Collections.Generic;
using System.Linq;

namespace Pkgdef_CSharp
{
    internal class PkgdefRegistryKeyDataItemSegment : PkgdefSegment
    {
        private readonly IReadOnlyList<PkgdefToken> tokens;
        private readonly PkgdefRegistryKeyDataItemNameSegment nameSegment;

        public PkgdefRegistryKeyDataItemSegment(IReadOnlyList<PkgdefToken> tokens)
        {
            PreCondition.AssertNotNullAndNotEmpty(tokens, nameof(tokens));
            PreCondition.AssertOneOf(tokens.First().GetTokenType(), new[] { PkgdefTokenType.AtSign, PkgdefTokenType.DoubleQuote }, "tokens.First().GetTokenType()");

            this.tokens = tokens;

            List<PkgdefToken> nameSegmentTokens = new List<PkgdefToken>() { tokens[0] };
            if (tokens[0].GetTokenType() == PkgdefTokenType.DoubleQuote)
            {
                for (int i = 1; i < tokens.Count; i++)
                {
                    nameSegmentTokens.Add(tokens[i]);
                    if (tokens[i].GetTokenType() == PkgdefTokenType.DoubleQuote)
                    {
                        break;
                    }
                }
            }
            this.nameSegment = new PkgdefRegistryKeyDataItemNameSegment(nameSegmentTokens);
        }

        public PkgdefRegistryKeyDataItemNameSegment GetNameSegment()
        {
            return this.nameSegment;
        }

        /// <inheritdoc/>
        public override int GetLength()
        {
            return PkgdefToken.GetLength(this.tokens);
        }

        /// <inheritdoc/>
        public override PkgdefSegmentType GetSegmentType()
        {
            return PkgdefSegmentType.RegistryKeyDataItem;
        }

        /// <inheritdoc/>
        public override int GetStartIndex()
        {
            return PkgdefToken.GetStartIndex(this.tokens);
        }

        /// <inheritdoc/>
        public override int GetAfterEndIndex()
        {
            return PkgdefToken.GetAfterEndIndex(this.tokens);
        }

        /// <inheritdoc/>
        public override int GetEndIndex()
        {
            return PkgdefToken.GetEndIndex(this.tokens);
        }

        /// <inheritdoc/>
        public override string GetText()
        {
            return string.Join("", this.tokens.Select((PkgdefToken token) => token.GetText()));
        }

        /// <inheritdoc/>
        public override bool Equals(object rhs)
        {
            return this.Equals(rhs as PkgdefRegistryKeyDataItemSegment);
        }

        public bool Equals(PkgdefRegistryKeyDataItemSegment rhs)
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
