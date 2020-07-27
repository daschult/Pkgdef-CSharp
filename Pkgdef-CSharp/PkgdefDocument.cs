using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// An abstract syntax tree (AST) for a parsed PKGDEF document.
    /// </summary>
    internal class PkgdefDocument
    {
        /// <summary>
        /// Parse a PkgdefDocument from the provided text.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="onIssue">The action that will be invoked if any issues are found during
        /// parsing.</param>
        /// <returns>The parsed PkgdefDocument.</returns>
        public static PkgdefDocument Parse(string text, Action<ParseIssue> onIssue)
        {
            return null;
        }
    }
}
