using System;
using System.Collections.Generic;
using System.Text;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// An iterator that converts characters to PkgdefTokens.
    /// </summary>
    internal class PkgdefTokenizer : IteratorBase<PkgdefToken>
    {
        private readonly CurrentIndexIterator<char> characters;
        private readonly Action<PkgdefIssue> onIssue;
        private readonly Queue<PkgdefToken> tokenQueue;
        private bool hasStarted;

        private PkgdefTokenizer(CurrentIndexIterator<char> characters, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(characters, nameof(characters));
            PreCondition.AssertNotNull(onIssue, nameof(onIssue));

            this.characters = characters;
            this.onIssue = onIssue;
            this.tokenQueue = new Queue<PkgdefToken>();
        }

        public static PkgdefTokenizer Create(string text, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(text, "text");

            return PkgdefTokenizer.Create(Iterator.Create(text), onIssue);
        }

        public static PkgdefTokenizer Create(int startIndex, string text, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertNotNull(text, "text");

            return PkgdefTokenizer.Create(startIndex, Iterator.Create(text), onIssue);
        }

        public static PkgdefTokenizer Create(Iterator<char> characters, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(characters, "characters");

            return new PkgdefTokenizer(CurrentIndexIterator.Create(characters), onIssue);
        }

        public static PkgdefTokenizer Create(int startIndex, Iterator<char> characters, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertNotNull(characters, "characters");

            return new PkgdefTokenizer(CurrentIndexIterator.Create(startIndex, characters), onIssue);
        }

        /// <inheritdoc/>
        public override PkgdefToken Current
        {
            get
            {
                PreCondition.AssertTrue(this.HasCurrent(), "this.HasCurrent()");

                return this.tokenQueue.Peek();
            }
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            this.characters.Dispose();
            this.tokenQueue.Clear();
        }

        /// <inheritdoc/>
        public override bool HasCurrent()
        {
            return this.tokenQueue.Count > 0;
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

            if (this.HasCurrent())
            {
                this.tokenQueue.Dequeue();
            }

            if (this.characters.HasCurrent())
            {
                switch (this.characters.GetCurrent())
                {
                    case '/':
                        this.tokenQueue.Enqueue(PkgdefToken.ForwardSlash(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '\\':
                        this.tokenQueue.Enqueue(PkgdefToken.Backslash(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '\"':
                        this.tokenQueue.Enqueue(PkgdefToken.DoubleQuote(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '@':
                        this.tokenQueue.Enqueue(PkgdefToken.AtSign(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '\n':
                        this.tokenQueue.Enqueue(PkgdefToken.NewLine(this.characters.GetCurrentIndex(), "\n"));
                        this.characters.Next();
                        break;

                    case '=':
                        this.tokenQueue.Enqueue(PkgdefToken.EqualsSign(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '$':
                        this.tokenQueue.Enqueue(PkgdefToken.DollarSign(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '[':
                        this.tokenQueue.Enqueue(PkgdefToken.LeftSquareBracket(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case ']':
                        this.tokenQueue.Enqueue(PkgdefToken.RightSquareBracket(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case ':':
                        this.tokenQueue.Enqueue(PkgdefToken.Colon(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '{':
                        this.tokenQueue.Enqueue(PkgdefToken.LeftCurlyBracket(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '}':
                        this.tokenQueue.Enqueue(PkgdefToken.RightCurlyBracket(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    case '-':
                        this.tokenQueue.Enqueue(PkgdefToken.Dash(this.characters.GetCurrentIndex()));
                        this.characters.Next();
                        break;

                    default:
                        if (PkgdefTokenizer.IsWhitespaceCharacter(this.characters.GetCurrent()))
                        {
                            this.ReadWhitespace();
                        }
                        else if (PkgdefTokenizer.IsLetter(this.characters.GetCurrent()))
                        {
                            this.ReadLetters();
                        }
                        else if (PkgdefTokenizer.IsDigit(this.characters.GetCurrent()))
                        {
                            this.ReadDigits();
                        }
                        else
                        {
                            this.tokenQueue.Enqueue(PkgdefToken.Unrecognized(characters.GetCurrentIndex(), characters.GetCurrent()));
                            this.characters.Next();
                        }
                        break;
                }
            }

            return this.HasCurrent();
        }

        private void ReadWhitespace()
        {
            PreCondition.AssertTrue(this.characters.HasCurrent(), "this.characters.HasCurrent()");
            PreCondition.AssertTrue(PkgdefTokenizer.IsWhitespaceCharacter(this.characters.GetCurrent()), "PkgdefTokenizer.IsWhitespaceCharacter(this.characters.GetCurrent())");

            PkgdefToken newLineToken = null;
            int startIndex = this.characters.GetCurrentIndex();
            StringBuilder builder = new StringBuilder();
            while (this.characters.HasCurrent() && PkgdefTokenizer.IsWhitespaceCharacter(this.characters.GetCurrent()))
            {
                if (this.characters.GetCurrent() != '\r')
                {
                    builder.Append(this.characters.TakeCurrent());
                }
                else
                {
                    int newLineStartIndex = this.characters.GetCurrentIndex();
                    if (this.characters.Next() && this.characters.GetCurrent() == '\n')
                    {
                        newLineToken = PkgdefToken.NewLine(newLineStartIndex, "\r\n");
                        this.characters.Next();
                        break;
                    }
                    else
                    {
                        builder.Append('\r');
                    }
                }
            }

            if (builder.Length > 0)
            {
                this.tokenQueue.Enqueue(PkgdefToken.Whitespace(startIndex, builder.ToString()));
            }
            if (newLineToken != null)
            {
                this.tokenQueue.Enqueue(newLineToken);
            }
        }

        private void ReadLetters()
        {
            PreCondition.AssertTrue(this.characters.HasCurrent(), "this.characters.HasCurrent()");
            PreCondition.AssertTrue(PkgdefTokenizer.IsLetter(this.characters.GetCurrent()), "PkgdefTokenizer.IsLetter(this.characters.GetCurrent())");

            int startIndex = this.characters.GetCurrentIndex();
            StringBuilder builder = new StringBuilder().Append(this.characters.TakeCurrent());
            while (this.characters.HasCurrent() && PkgdefTokenizer.IsLetter(this.characters.GetCurrent()))
            {
                builder.Append(this.characters.TakeCurrent());
            }

            this.tokenQueue.Enqueue(PkgdefToken.Letters(startIndex, builder.ToString()));
        }

        private void ReadDigits()
        {
            PreCondition.AssertTrue(this.characters.HasCurrent(), "this.characters.HasCurrent()");
            PreCondition.AssertTrue(PkgdefTokenizer.IsDigit(this.characters.GetCurrent()), "PkgdefTokenizer.IsDigit(this.characters.GetCurrent())");

            int startIndex = this.characters.GetCurrentIndex();
            StringBuilder builder = new StringBuilder().Append(this.characters.TakeCurrent());
            while (this.characters.HasCurrent() && PkgdefTokenizer.IsDigit(this.characters.GetCurrent()))
            {
                builder.Append(this.characters.TakeCurrent());
            }

            this.tokenQueue.Enqueue(PkgdefToken.Digits(startIndex, builder.ToString()));
        }

        public static bool IsWhitespaceCharacter(char character)
        {
            return character == ' ' || character == '\t' || character == '\r';
        }

        public static bool IsLetter(char character)
        {
            return char.IsLetter(character);
        }

        public static bool IsDigit(char character)
        {
            return char.IsDigit(character);
        }
    }
}
