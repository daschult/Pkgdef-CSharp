﻿using System;
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
        private readonly IReadOnlyList<PkgdefSegment> segments;
        private readonly IReadOnlyList<PkgdefIssue> issues;

        private PkgdefDocument(IReadOnlyList<PkgdefSegment> segments, IReadOnlyList<PkgdefIssue> issues)
        {
            PreCondition.AssertNotNull(segments, nameof(segments));

            this.segments = segments;
            this.issues = issues ?? new List<PkgdefIssue>();
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

        internal static PkgdefSegment ParseSegment(PkgdefTokenizer tokenizer, Action<PkgdefIssue> onIssue)
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
    }
}
