namespace Pkgdef_CSharp
{
    /// <summary>
    /// The different types of segments within a PKGDEF document.
    /// </summary>
    internal enum PkgdefSegmentType
    {
        /// <summary>
        /// A comment that begins with "//".
        /// </summary>
        LineComment,

        /// <summary>
        /// Whitespace characters.
        /// </summary>
        Whitespace,
    }
}
