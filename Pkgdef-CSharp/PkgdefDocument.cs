using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pkgdef_CSharp
{
    /// <summary>
    /// An abstract syntax tree (AST) for a parsed PKGDEF document.
    /// </summary>
    internal class PkgdefDocument
    {
        internal static void ThrowParseException(PkgdefIssue issue)
        {
            throw new ParseException(issue.GetMessage());
        }

        internal static void IgnoreIssue(PkgdefIssue issue)
        {
        }

        private readonly IReadOnlyList<PkgdefSegment> segments;
        private readonly IReadOnlyList<PkgdefIssue> issues;

        private PkgdefDocument(IReadOnlyList<PkgdefSegment> segments, IReadOnlyList<PkgdefIssue> issues)
        {
            PreCondition.AssertNotNull(segments, nameof(segments));
            PreCondition.AssertNotNull(issues, nameof(issues));

            this.segments = segments;
            this.issues = issues;
        }

        /// <summary>
        /// Get the text of this PkgdefDocument.
        /// </summary>
        /// <returns>The text of this PkgdefDocument.</returns>
        public string GetText()
        {
            return string.Join("", this.segments.Select((PkgdefSegment segment) => segment.GetText()));
        }

        /// <summary>
        /// Get the segments of this PkgdefDocument.
        /// </summary>
        /// <returns>The segments of this PkgdefDocument.</returns>
        public IReadOnlyList<PkgdefSegment> GetSegments()
        {
            return this.segments;
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
        /// <param name="iterator">The characters to parse.</param>
        /// <returns>The parsed PkgdefDocument.</returns>
        public static PkgdefDocument Parse(Iterator<char> iterator)
        {
            PreCondition.AssertNotNull(iterator, nameof(iterator));

            List<PkgdefIssue> issues = new List<PkgdefIssue>();
            PkgdefTokenizer tokenizer = PkgdefTokenizer.Create(iterator, onIssue: issues.Add);

            tokenizer.EnsureHasStarted();

            List<PkgdefSegment> segments = new List<PkgdefSegment>();
            while (tokenizer.HasCurrent())
            {
                segments.Add(PkgdefDocument.ParseSegment(tokenizer, onIssue: issues.Add));
            }

            return new PkgdefDocument(segments, issues);
        }

        private static PkgdefSegment ParseSegment(PkgdefTokenizer tokenizer, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(tokenizer, nameof(tokenizer));
            PreCondition.AssertTrue(tokenizer.HasCurrent(), "tokenizer.HasCurrent()");
            PreCondition.AssertNotNull(onIssue, nameof(onIssue));

            PkgdefSegment result;
            switch (tokenizer.GetCurrent().GetTokenType())
            {
                case PkgdefTokenType.Whitespace:
                    result = PkgdefSegment.Whitespace(tokenizer.GetCurrent().GetStartIndex(), tokenizer.GetCurrent().GetText());
                    tokenizer.Next();
                    break;

                case PkgdefTokenType.NewLine:
                    result = PkgdefSegment.NewLine(tokenizer.GetCurrent().GetStartIndex(), tokenizer.GetCurrent().GetText());
                    tokenizer.Next();
                    break;

                case PkgdefTokenType.ForwardSlash:
                    result = PkgdefDocument.ParseLineComment(tokenizer, onIssue);
                    break;

                case PkgdefTokenType.LeftSquareBracket:
                    result = PkgdefDocument.ParseRegistryKeyPath(tokenizer, onIssue);
                    break;

                case PkgdefTokenType.AtSign:
                case PkgdefTokenType.DoubleQuote:
                    result = PkgdefDocument.ParseRegistryKeyDataItem(tokenizer, onIssue);
                    break;

                default:
                    result = PkgdefSegment.Unrecognized(tokenizer.GetCurrent().GetStartIndex(), tokenizer.GetCurrent().GetText());
                    tokenizer.Next();
                    break;
            }

            return result;
        }

        internal static PkgdefSegment ParseLineComment(PkgdefTokenizer tokenizer, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(tokenizer, nameof(tokenizer));
            PreCondition.AssertTrue(tokenizer.HasCurrent(), "tokenizer.HasCurrent()");
            PreCondition.AssertEqual(tokenizer.GetCurrent().GetTokenType(), PkgdefTokenType.ForwardSlash, "tokenizer.GetCurrent().GetTokenType()");
            PreCondition.AssertNotNull(onIssue, nameof(onIssue));

            PkgdefSegment result;
            int startIndex = tokenizer.TakeCurrent().GetStartIndex();
            StringBuilder builder = new StringBuilder().Append('/');

            if (!tokenizer.HasCurrent())
            {
                onIssue(new PkgdefIssue(startIndex, 1, "Missing line-comment's second forward slash ('/')."));
                result = PkgdefSegment.Unrecognized(startIndex, builder.ToString());
            }
            else if (tokenizer.GetCurrent().GetTokenType() != PkgdefTokenType.ForwardSlash)
            {
                onIssue.Invoke(new PkgdefIssue(startIndex + 1, 1, "Expected line-comment's second forward slash ('/')."));
                result = PkgdefSegment.Unrecognized(startIndex, builder.ToString());
            }
            else
            {
                builder.Append('/');
                tokenizer.Next();

                while (tokenizer.HasCurrent() && tokenizer.GetCurrent().GetTokenType() != PkgdefTokenType.NewLine)
                {
                    builder.Append(tokenizer.TakeCurrent().GetText());
                }
                string text = builder.ToString();
                result = PkgdefSegment.LineComment(startIndex, text);
            }

            return result;
        }

        internal static PkgdefRegistryKeyPathSegment ParseRegistryKeyPath(int startIndex, string text)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertNotNullAndNotEmpty(text, nameof(text));
            PreCondition.AssertEqual(text[0], '[', "text[0]");

            Action<PkgdefIssue> onIssue = PkgdefDocument.IgnoreIssue;
            PkgdefTokenizer tokenizer = PkgdefTokenizer.Create(startIndex, text, onIssue);
            tokenizer.Next();
            return PkgdefDocument.ParseRegistryKeyPath(tokenizer, onIssue);
        }

        internal static PkgdefRegistryKeyPathSegment ParseRegistryKeyPath(PkgdefTokenizer tokenizer, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(tokenizer, nameof(tokenizer));
            PreCondition.AssertTrue(tokenizer.HasCurrent(), "tokenizer.HasCurrent()");
            PreCondition.AssertEqual(tokenizer.GetCurrent().GetTokenType(), PkgdefTokenType.LeftSquareBracket, "tokenizer.GetCurrent().GetTokenType()");
            PreCondition.AssertNotNull(onIssue, nameof(onIssue));

            List<PkgdefToken> tokens = new List<PkgdefToken>() { tokenizer.TakeCurrent() };
            while (tokenizer.HasCurrent())
            {
                PkgdefTokenType tokenType = tokenizer.GetCurrent().GetTokenType();
                if (tokenType == PkgdefTokenType.NewLine)
                {
                    break;
                }
                else
                {
                    tokens.Add(tokenizer.TakeCurrent());
                    if (tokenType == PkgdefTokenType.RightSquareBracket)
                    {
                        break;
                    }
                }
            }

            PkgdefRegistryKeyPathSegment result = new PkgdefRegistryKeyPathSegment(tokens);
            if (result.GetRightSquareBracket() == null)
            {
                onIssue(new PkgdefIssue(result.GetStartIndex(), result.GetLength(), "Missing registry key path right square bracket (']')."));
            }

            return result;
        }

        internal static PkgdefRegistryKeyDataItemSegment ParseRegistryKeyDataItem(int startIndex, string text)
        {
            PreCondition.AssertGreaterThanOrEqualTo(startIndex, 0, nameof(startIndex));
            PreCondition.AssertNotNullAndNotEmpty(text, nameof(text));
            PreCondition.AssertOneOf(text[0], "@\"", "text[0]");

            Action<PkgdefIssue> onIssue = PkgdefDocument.IgnoreIssue;
            PkgdefTokenizer tokenizer = PkgdefTokenizer.Create(startIndex, text, onIssue);
            tokenizer.Next();
            return PkgdefDocument.ParseRegistryKeyDataItem(tokenizer, onIssue);
        }

        internal static PkgdefRegistryKeyDataItemSegment ParseRegistryKeyDataItem(PkgdefTokenizer tokenizer, Action<PkgdefIssue> onIssue)
        {
            PreCondition.AssertNotNull(tokenizer, nameof(tokenizer));
            PreCondition.AssertTrue(tokenizer.HasCurrent(), "tokenizer.HasCurrent()");
            PreCondition.AssertOneOf(tokenizer.GetCurrent().GetTokenType(), new[] { PkgdefTokenType.AtSign, PkgdefTokenType.DoubleQuote }, "tokenizer.GetCurrent().GetTokenType()");
            PreCondition.AssertNotNull(onIssue, nameof(onIssue));

            PkgdefToken registryKeyDataItemNameFirstToken = tokenizer.TakeCurrent();
            List<PkgdefToken> tokens = new List<PkgdefToken>() { registryKeyDataItemNameFirstToken };
            bool dataItemDone = false;

            if (registryKeyDataItemNameFirstToken.GetTokenType() == PkgdefTokenType.DoubleQuote)
            {
                if (!tokenizer.HasCurrent())
                {
                    onIssue(new PkgdefIssue(registryKeyDataItemNameFirstToken.GetStartIndex(), registryKeyDataItemNameFirstToken.GetLength(), "Missing registry key data item name closing double-quote ('\"')."));
                    dataItemDone = true;
                }
                else
                {
                    while (tokenizer.HasCurrent())
                    {
                        PkgdefTokenType tokenType = tokenizer.GetCurrent().GetTokenType();
                        if (tokenType == PkgdefTokenType.NewLine)
                        {
                            onIssue(new PkgdefIssue(PkgdefToken.GetStartIndex(tokens), PkgdefToken.GetLength(tokens), "Missing registry key data item name closing double-quote ('\"')."));
                            dataItemDone = true;
                            break;
                        }
                        else
                        {
                            PkgdefToken token = tokenizer.TakeCurrent();
                            tokens.Add(token);
                            if (token.GetTokenType() == PkgdefTokenType.DoubleQuote)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (!dataItemDone)
            {
                if (!tokenizer.HasCurrent())
                {
                    onIssue(new PkgdefIssue(PkgdefToken.GetStartIndex(tokens), PkgdefToken.GetLength(tokens), "Missing registry key data item equals sign ('=')."));
                    dataItemDone = true;
                }
                else
                {
                    while (tokenizer.HasCurrent())
                    {
                        PkgdefTokenType tokenType = tokenizer.GetCurrent().GetTokenType();
                        if (tokenType == PkgdefTokenType.NewLine)
                        {
                            onIssue(new PkgdefIssue(PkgdefToken.GetStartIndex(tokens), PkgdefToken.GetLength(tokens), "Missing registry key data item equals sign ('=')."));
                            dataItemDone = true;
                            break;
                        }
                        else
                        {
                            PkgdefToken token = tokenizer.TakeCurrent();
                            tokens.Add(token);
                            if (token.GetTokenType() == PkgdefTokenType.EqualsSign)
                            {
                                break;
                            }
                            else if (token.GetTokenType() != PkgdefTokenType.Whitespace)
                            {
                                onIssue(new PkgdefIssue(token.GetStartIndex(), token.GetLength(), "Expected registry key data item equals sign ('=')."));
                            }
                        }
                    }
                }
            }

            if (!dataItemDone)
            {
                if (!tokenizer.HasCurrent())
                {
                    onIssue(new PkgdefIssue(PkgdefToken.GetStartIndex(tokens), PkgdefToken.GetLength(tokens), "Missing registry key data item value."));
                }
                else
                {
                    int dataItemValueTokenCount = 0;
                    while (tokenizer.HasCurrent())
                    {
                        PkgdefTokenType tokenType = tokenizer.GetCurrent().GetTokenType();
                        if (tokenType == PkgdefTokenType.NewLine)
                        {
                            if (dataItemValueTokenCount == 0)
                            {
                                onIssue(new PkgdefIssue(PkgdefToken.GetStartIndex(tokens), PkgdefToken.GetLength(tokens), "Missing registry key data item value."));
                            }
                            break;
                        }
                        else
                        {
                            PkgdefToken token = tokenizer.TakeCurrent();
                            tokens.Add(token);
                            if (token.GetTokenType() != PkgdefTokenType.Whitespace)
                            {
                                dataItemValueTokenCount++;
                            }
                        }
                    }
                }
            }

            return new PkgdefRegistryKeyDataItemSegment(tokens);
        }
    }
}
