namespace Pkgdef_CSharp
{
    /// <summary>
    /// The different types of tokens within a PKGDEF document.
    /// </summary>
    internal enum PkgdefTokenType
    {
        /// <summary>
        /// A comment that begins with "//".
        /// </summary>
        LineComment,

        /// <summary>
        /// A token made up of whitespace characters.
        /// </summary>
        Whitespace,

        /// <summary>
        /// A token for a quoted string.
        /// </summary>
        QuotedString,

        /// <summary>
        /// The "at" sign ('@').
        /// </summary>
        AtSign,

        /// <summary>
        /// An unrecognized token.
        /// </summary>
        Unrecognized,
    }
}
