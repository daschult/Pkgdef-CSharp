using System;
using System.Text;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// An iterator that converts characters to PkgdefTokens.
    /// </summary>
    internal class PkgdefTokenizer : IteratorBase<PkgdefToken>
    {
        private readonly Iterator<char> characters;
        private readonly Action<PkgdefIssue> onIssue;
        private int currentCharacterIndex;
        private PkgdefToken currentToken;
        private bool hasStarted;

        private PkgdefTokenizer(Iterator<char> characters, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(characters, nameof(characters));
            PreCondition.AssertNotNull(onIssue, nameof(onIssue));

            this.characters = characters;
            this.onIssue = onIssue;
            this.currentCharacterIndex = 0;
        }

        public static PkgdefTokenizer Create(string text, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(text, "text");

            return PkgdefTokenizer.Create(Iterator.Create(text), onIssue);
        }

        public static PkgdefTokenizer Create(Iterator<char> characters, Action<PkgdefIssue> onIssue)
        {
            return new PkgdefTokenizer(characters, onIssue);
        }

        /// <inheritdoc/>
        public override PkgdefToken Current
        {
            get
            {
                PreCondition.AssertTrue(this.HasCurrent(), "this.HasCurrent()");

                return this.currentToken;
            }
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            this.characters.Dispose();
        }

        /// <inheritdoc/>
        public override bool HasCurrent()
        {
            return this.currentToken != null;
        }

        /// <inheritdoc/>
        public override bool HasStarted()
        {
            return this.hasStarted;
        }

        /// <inheritdoc/>
        public override bool MoveNext()
        {
            this.characters.EnsureHasStarted();
            this.hasStarted = true;

            if (!this.HasCurrentCharacter())
            {
                this.currentToken = null;
            }
            else
            {
                switch (this.GetCurrentCharacter())
                {
                    case '/':
                        this.currentToken = this.ReadLineComment();
                        break;

                    case '\"':
                        this.currentToken = this.ReadQuotedString();
                        break;

                    case '@':
                        this.currentToken = PkgdefToken.AtSign(this.currentCharacterIndex);
                        this.NextCharacter();
                        break;

                    default:
                        if (PkgdefTokenizer.IsWhitespaceCharacter(this.GetCurrentCharacter()))
                        {
                            this.currentToken = this.ReadWhitespace();
                        }
                        else
                        {
                            this.currentToken = new PkgdefToken(
                                this.currentCharacterIndex,
                                this.GetCurrentCharacter(),
                                PkgdefTokenType.Unrecognized);
                            this.NextCharacter();
                        }
                        break;
                }
            }

            return this.HasCurrent();
        }

        private bool HasCurrentCharacter()
        {
            return this.characters.HasCurrent();
        }

        private char GetCurrentCharacter()
        {
            PreCondition.AssertTrue(this.HasCurrentCharacter(), "this.HasCurrentCharacter()");

            return this.characters.Current;
        }

        private bool NextCharacter()
        {
            bool result = this.characters.Next();
            if (result)
            {
                this.currentCharacterIndex++;
            }
            return result;
        }

        private char TakeCurrentCharacter()
        {
            char result = this.GetCurrentCharacter();
            this.NextCharacter();

            return result;
        }

        /// <summary>
        /// Get whether or not the provided character is a whitespace character.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <returns>Whether or not the provided character is a whitespace character.</returns>
        internal static bool IsWhitespaceCharacter(char character)
        {
            return character == ' ' || character == '\t' || character == '\r' || character == '\n';
        }

        private PkgdefToken ReadWhitespace()
        {
            PreCondition.AssertTrue(this.HasCurrentCharacter(), "this.HasCurrentCharacter()");
            PreCondition.AssertTrue(PkgdefTokenizer.IsWhitespaceCharacter(this.GetCurrentCharacter()), "PkgdefTokenizer.IsWhitespaceCharacter(this.GetCurrentCharacter())");

            int startIndex = this.currentCharacterIndex;
            StringBuilder builder = new StringBuilder().Append(this.TakeCurrentCharacter());
            while (this.HasCurrentCharacter() && PkgdefTokenizer.IsWhitespaceCharacter(this.GetCurrentCharacter()))
            {
                builder.Append(this.TakeCurrentCharacter());
            }
            string text = builder.ToString();

            return PkgdefToken.Whitespace(startIndex, text);
        }

        private PkgdefToken ReadLineComment()
        {
            PreCondition.AssertTrue(this.HasCurrentCharacter(), "this.HasCurrentCharacter()");
            PreCondition.AssertEqual(this.GetCurrentCharacter(), '/', "this.GetCurrentCharacter()");

            PkgdefToken result;

            int startIndex = this.currentCharacterIndex;
            StringBuilder builder = new StringBuilder().Append(this.TakeCurrentCharacter());
            if (!this.HasCurrentCharacter())
            {
                onIssue.Invoke(new PkgdefIssue(startIndex, 1, "Missing the line-comment's second forward slash ('/')."));
                result = PkgdefToken.Unrecognized(startIndex, builder.ToString());
            }
            else if (this.GetCurrentCharacter() != '/')
            {
                onIssue.Invoke(new PkgdefIssue(startIndex + 1, 1, "Expected the line-comment's second forward slash ('/')."));
                result = PkgdefToken.Unrecognized(startIndex, builder.ToString());
            }
            else
            {
                builder.Append(this.TakeCurrentCharacter());
                while (this.HasCurrentCharacter())
                {
                    char character = this.TakeCurrentCharacter();
                    builder.Append(character);
                    if (character == '\n')
                    {
                        break;
                    }
                }
                string text = builder.ToString();

                result = PkgdefToken.LineComment(startIndex, text);
            }

            return result;
        }

        private PkgdefToken ReadQuotedString()
        {
            PreCondition.AssertTrue(this.HasCurrentCharacter(), "this.HasCurrentCharacter()");
            PreCondition.AssertEqual(this.GetCurrentCharacter(), '\"', "this.GetCurrentCharacter()");

            int startIndex = this.currentCharacterIndex;
            StringBuilder builder = new StringBuilder().Append(this.TakeCurrentCharacter());
            bool closed = false;
            while (this.HasCurrentCharacter())
            {
                char character = this.GetCurrentCharacter();
                if (character == '\n')
                {
                    break;
                }
                builder.Append(character);
                this.NextCharacter();
                if (character == '\"')
                {
                    closed = true;
                    break;
                }
            }

            string text = builder.ToString();
            if (!closed)
            {
                onIssue.Invoke(PkgdefIssue.Error(startIndex, text.Length, "Missing quoted-string's closing double-quote ('\"')."));
            }
            
            return PkgdefToken.QuotedString(startIndex, text);
        }
    }
}
