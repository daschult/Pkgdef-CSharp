namespace Pkgdef_CSharp
{
    /// <summary>
    /// The different types of tokens within a PKGDEF document.
    /// </summary>
    internal enum PkgdefTokenType
    {
        /// <summary>
        /// A forward-slash character ('/')
        /// </summary>
        ForwardSlash,

        /// <summary>
        /// A backslash character ('\').
        /// </summary>
        Backslash,

        /// <summary>
        /// A token made up of whitespace characters.
        /// </summary>
        Whitespace,

        /// <summary>
        /// A token for a newline or carriage-return newline.
        /// </summary>
        NewLine,

        /// <summary>
        /// The "at" sign ('@').
        /// </summary>
        AtSign,

        /// <summary>
        /// The equals sign ('=').
        /// </summary>
        EqualsSign,

        /// <summary>
        /// The dollar sign ('$').
        /// </summary>
        DollarSign,

        /// <summary>
        /// The left square bracket ('[').
        /// </summary>
        LeftSquareBracket,

        /// <summary>
        /// The right square bracket (']').
        /// </summary>
        RightSquareBracket,

        /// <summary>
        /// A double-quote ('"').
        /// </summary>
        DoubleQuote,

        /// <summary>
        /// A colon (':').
        /// </summary>
        Colon,

        /// <summary>
        /// A left-curly bracket ('{').
        /// </summary>
        LeftCurlyBracket,

        /// <summary>
        /// A right-curly bracket ('}').
        /// </summary>
        RightCurlyBracket,

        /// <summary>
        /// A dash ('-').
        /// </summary>
        Dash,

        /// <summary>
        /// A sequence of letters.
        /// </summary>
        Letters,

        /// <summary>
        /// A sequence of digits.
        /// </summary>
        Digits,

        /// <summary>
        /// An unrecognized token.
        /// </summary>
        Unrecognized,
    }
}
