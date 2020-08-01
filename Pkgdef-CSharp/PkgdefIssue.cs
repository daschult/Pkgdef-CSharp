using System.Text;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// An issue that occurs while parsing a PKGDEF document.
    /// </summary>
    internal class PkgdefIssue
    {
        private readonly int startIndex;
        private readonly int length;
        private readonly string message;

        /// <summary>
        /// Create a new PkgdefIssue object.
        /// </summary>
        /// <param name="startIndex">The character index that the issue starts on.</param>
        /// <param name="length">The number of characters that the issue spans over.</param>
        /// <param name="message">The message that describes the issue.</param>
        public PkgdefIssue(int startIndex, int length, string message)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertGreaterThanOrEqualTo(length, 1, nameof(length));
            PreCondition.AssertNotNullAndNotEmpty(message, nameof(message));

            this.startIndex = startIndex;
            this.length = length;
            this.message = message;
        }

        /// <summary>
        /// Create a new PkgdefIssue error object.
        /// </summary>
        /// <param name="startIndex">The character index that the issue starts on.</param>
        /// <param name="length">The number of characters that the issue spans over.</param>
        /// <param name="message">The message that describes the issue.</param>
        public static PkgdefIssue Error(int startIndex, int length, string message)
        {
            return new PkgdefIssue(startIndex, length, message);
        }

        /// <summary>
        /// Get the character index that the issue starts on.
        /// </summary>
        /// <returns>The character index that the issue starts on.</returns>
        public int GetStartIndex()
        {
            return this.startIndex;
        }

        /// <summary>
        /// Get the number of characters that the issue spans over.
        /// </summary>
        /// <returns>The number of characters that the issue spans over.</returns>
        public int GetLength()
        {
            return this.length;
        }

        /// <summary>
        /// Get the message that describes the issue.
        /// </summary>
        /// <returns>The message that describes the issue.</returns>
        public string GetMessage()
        {
            return this.message;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('{');

            builder.Append($"\"startIndex\":{this.GetStartIndex()}");

            builder.Append(',');
            builder.Append($"\"length\":{this.GetLength()}");

            builder.Append(',');
            builder.Append($"\"message\":\"{Strings.Escape(this.GetMessage())}\"");

            builder.Append('}');

            return builder.ToString();
        }

        /// <inheritdoc/>
        public override bool Equals(object rhs)
        {
            return this.Equals(rhs as PkgdefIssue);
        }

        public bool Equals(PkgdefIssue rhs)
        {
            return rhs != null &&
                this.startIndex == rhs.startIndex &&
                this.length == rhs.length &&
                this.message == rhs.message;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.startIndex.GetHashCode() ^
                this.length.GetHashCode() ^
                this.message.GetHashCode();
        }
    }
}
