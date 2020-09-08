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
        /// Whitespace characters.there are my fake changes
        /// </summary>
        Whitespace,

        /// <summary>
        /// A newline character or a carriage-return newline sequence.
        /// </summary>
        NewLine,

        /// <summary>
        /// A quoted-string.
        /// </summary>
        QuotedString,

        /// <summary>
        /// A token that will be substituted for a different piece of text at "runtime".
        /// </summary>
        SubstitutionToken,

        /// <summary>
        /// A GUID.
        /// </summary>
        Guid,

        /// <summary>
        /// A registry key path and each of the key's data items.
        /// </summary>
        RegistryKey,

        /// <summary>
        /// The path to a registry key.
        /// </summary>
        RegistryKeyPath,

        /// <summary>
        /// A data item within a registry key.
        /// </summary>
        RegistryKeyDataItem,

        /// <summary>
        /// The name of a registry key data item.
        /// </summary>
        RegistryKeyDataItemName,

        /// <summary>
        /// An unrecognized segment.
        /// </summary>
        Unrecognized,
    }
}
