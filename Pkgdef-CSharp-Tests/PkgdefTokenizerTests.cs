using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pkgdef_CSharp;

namespace Pkgdef_CSharp_Tests
{
    [TestClass]
    public class PkgdefTokenizerTests
    {
        [TestMethod]
        public void Create_WithString()
        {
            void CreateTest(string text, PkgdefToken[] expectedTokens = null, PkgdefIssue[] expectedIssues = null, Exception expectedException = null)
            {
                List<PkgdefIssue> issues = new List<PkgdefIssue>();
                if (expectedException != null)
                {
                    AssertEx.Throws(() => PkgdefTokenizer.Create(text, issues.Add), expectedException);
                }
                else
                {
                    PkgdefTokenizer tokenizer = PkgdefTokenizer.Create(text, issues.Add);
                    Assert.IsNotNull(tokenizer);
                    Assert.IsFalse(tokenizer.HasStarted());
                    Assert.IsFalse(tokenizer.HasCurrent());

                    if (expectedTokens == null)
                    {
                        Assert.AreEqual(0, tokenizer.Count());
                    }
                    else
                    {
                        CollectionAssert.AreEqual(expectedTokens, tokenizer.ToArray());
                    }

                    if (expectedIssues == null)
                    {
                        Assert.AreEqual(0, issues.Count());
                    }
                    else
                    {
                        CollectionAssert.AreEqual(expectedIssues, issues);
                    }

                    Assert.IsTrue(tokenizer.HasStarted());
                    Assert.IsFalse(tokenizer.HasCurrent());
                }
            }

            CreateTest(
                null,
                expectedException: new PreConditionException("text cannot be null."));
            CreateTest(
                "",
                new PkgdefToken[0]);

            CreateTest(
                "/",
                new[] { PkgdefToken.ForwardSlash(0) });

            CreateTest(
                "\\",
                new[] { PkgdefToken.Backslash(0) });

            CreateTest(
                " ",
                new[] { PkgdefToken.Whitespace(0, " ") });
            CreateTest(
                "    ",
                new[] { PkgdefToken.Whitespace(0, "    ") });
            CreateTest(
                "\t",
                new[] { PkgdefToken.Whitespace(0, "\t") });
            CreateTest(
                "\t\t\t",
                new[] { PkgdefToken.Whitespace(0, "\t\t\t") });
            CreateTest(
                " \t \t \t ",
                new[] { PkgdefToken.Whitespace(0, " \t \t \t ") });
            CreateTest(
                "\r",
                new[] { PkgdefToken.Whitespace(0, "\r") });
            CreateTest(
                " \r ",
                new[] { PkgdefToken.Whitespace(0, " \r ") });

            CreateTest(
                "\n",
                new[] { PkgdefToken.NewLine(0, "\n") });
            CreateTest(
                "\r\n",
                new[] { PkgdefToken.NewLine(0, "\r\n") });

            CreateTest(
                "@",
                new[] { PkgdefToken.AtSign(0) });

            CreateTest(
                "=",
                new[] { PkgdefToken.EqualsSign(0) });

            CreateTest(
                "$",
                new[] { PkgdefToken.DollarSign(0) });

            CreateTest(
                "[",
                new[] { PkgdefToken.LeftSquareBracket(0) });

            CreateTest(
                "]",
                new[] { PkgdefToken.RightSquareBracket(0) });

            CreateTest(
                "\"",
                new[] { PkgdefToken.DoubleQuote(0) });

            CreateTest(
                ":",
                new[] { PkgdefToken.Colon(0) });

            CreateTest(
                "{",
                new[] { PkgdefToken.LeftCurlyBracket(0) });

            CreateTest(
                "}",
                new[] { PkgdefToken.RightCurlyBracket(0) });

            CreateTest(
                "-",
                new[] { PkgdefToken.Dash(0) });

            CreateTest(
                "a",
                new[] { PkgdefToken.Letters(0, "a") });
            CreateTest(
                "abc",
                new[] { PkgdefToken.Letters(0, "abc") });

            CreateTest(
                "1",
                new[] { PkgdefToken.Digits(0, "1") });
            CreateTest(
                "123",
                new[] { PkgdefToken.Digits(0, "123") });

            // Unrecognized tokens
            CreateTest(
                "&",
                new[] { PkgdefToken.Unrecognized(0, '&') });
            CreateTest(
                "&^%",
                new[]
                {
                    PkgdefToken.Unrecognized(0, '&'),
                    PkgdefToken.Unrecognized(1, '^'),
                    PkgdefToken.Unrecognized(2, '%'),
                });
        }
    }
}
