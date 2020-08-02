using System.Text;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// A collection of methods for working with strings.
    /// </summary>
    internal static class Strings
    {
        public static string EscapeAndQuote(string value)
        {
            return Strings.Quote(Strings.Escape(value));
        }

        public static string Quote(string value)
        {
            PreCondition.AssertNotNull(value, nameof(value));

            return $"\"{value}\"";
        }

        /// <summary>
        /// Escape all of the escape sequences within the provided string value.
        /// </summary>
        /// <param name="value">The value to escape.</param>
        /// <returns>The escaped string.</returns>
        public static string Escape(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                StringBuilder builder = new StringBuilder(value.Length);
                foreach (char character in value)
                {
                    switch (character)
                    {
                        case '\t':
                            builder.Append("\\t");
                            break;

                        case '\r':
                            builder.Append("\\r");
                            break;

                        case '\n':
                            builder.Append("\\n");
                            break;

                        case '\v':
                            builder.Append("\\v");
                            break;

                        case '\'':
                            builder.Append("\\'");
                            break;

                        case '\"':
                            builder.Append("\\\"");
                            break;

                        case '\0':
                            builder.Append("\\0");
                            break;

                        case '\f':
                            builder.Append("\\f");
                            break;

                        case '\b':
                            builder.Append("\\b");
                            break;

                        case '\a':
                            builder.Append("\\a");
                            break;

                        case '\\':
                            builder.Append("\\\\");
                            break;

                        default:
                            builder.Append(character);
                            break;
                    }
                }
                value = builder.ToString();
            }
            return value;
        }
    }
}
