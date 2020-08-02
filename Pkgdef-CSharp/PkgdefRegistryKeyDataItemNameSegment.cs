using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkgdef_CSharp
{
    internal class PkgdefRegistryKeyDataItemNameSegment : PkgdefSegment
    {
        private readonly IReadOnlyList<PkgdefToken> tokens;

        public PkgdefRegistryKeyDataItemNameSegment(IReadOnlyList<PkgdefToken> tokens)
        {
            PreCondition.AssertNotNullAndNotEmpty(tokens, nameof(tokens));
            PreCondition.AssertOneOf(tokens[0].GetTokenType(), new[] { PkgdefTokenType.AtSign, PkgdefTokenType.DoubleQuote }, "tokens[0].GetTokenType()");

            this.tokens = tokens;
        }

        public override int GetLength()
        {
            return PkgdefToken.GetLength(this.tokens);
        }

        public override PkgdefSegmentType GetSegmentType()
        {
            return PkgdefSegmentType.RegistryKeyDataItemName;
        }

        public override int GetStartIndex()
        {
            return PkgdefToken.GetStartIndex(this.tokens);
        }

        public override string GetText()
        {
            return PkgdefToken.GetText(this.tokens);
        }
    }
}
