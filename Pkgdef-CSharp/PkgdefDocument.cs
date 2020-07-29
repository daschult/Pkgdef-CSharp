using System.Collections.Generic;
using System.Linq;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// An abstract syntax tree (AST) for a parsed PKGDEF document.
    /// </summary>
    internal class PkgdefDocument
    {
        private readonly IReadOnlyList<PkgdefToken> tokens;
        private readonly IReadOnlyList<PkgdefIssue> issues;

        private PkgdefDocument(IReadOnlyList<PkgdefToken> tokens, IReadOnlyList<PkgdefIssue> issues)
        {
            PreCondition.AssertNotNull(tokens, nameof(tokens));

            this.tokens = tokens;
            this.issues = issues ?? new List<PkgdefIssue>();
        }

        /// <summary>
        /// Get the text of this PkgdefDocument.
        /// </summary>
        /// <returns>The text of this PkgdefDocument.</returns>
        public string GetText()
        {
            return string.Join("", this.tokens.Select((PkgdefToken token) => token.GetText()));
        }

        /// <summary>
        /// Get the tokens of this PkgdefDocument.
        /// </summary>
        /// <returns>The tokens of this PkgdefDocument.</returns>
        public IReadOnlyList<PkgdefToken> GetTokens()
        {
            return this.tokens;
        }

        /// <summary>
        /// Get the issues of this PkgdefDocument.
        /// </summary>
        /// <returns>The isues of this PkgdefDocument.</returns>
        public IReadOnlyList<PkgdefIssue> GetIssues()
        {
            return this.issues;
        }

        /// <summary>
        /// Parse a PkgdefDocument from the provided text.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The parsed PkgdefDocument.</returns>
        public static PkgdefDocument Parse(string text)
        {
            PreCondition.AssertNotNull(text, nameof(text));

            return PkgdefDocument.Parse(Iterator.Create(text));
        }

        /// <summary>
        /// Parse a PkgdefDocument from the provided characters.
        /// </summary>
        /// <param name="characters">The characters to parse.</param>
        /// <returns>The parsed PkgdefDocument.</returns>
        public static PkgdefDocument Parse(Iterator<char> characters)
        {
            PreCondition.AssertNotNull(characters, nameof(characters));

            List<PkgdefIssue> issues = new List<PkgdefIssue>();
            
            PkgdefTokenizer tokenizer = PkgdefTokenizer.Create(characters, onIssue: issues.Add);
            IReadOnlyList<PkgdefToken> tokens = tokenizer.ToList();

            return new PkgdefDocument(tokens, issues);
        }
    }
}
