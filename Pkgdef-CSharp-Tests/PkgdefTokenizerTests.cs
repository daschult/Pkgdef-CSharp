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

            // Unrecognized tokens
            CreateTest(
                "a",
                new[] { PkgdefToken.Unrecognized(0, "a") });
            CreateTest(
                "abc",
                new[]
                {
                    PkgdefToken.Unrecognized(0, "a"),
                    PkgdefToken.Unrecognized(1, "b"),
                    PkgdefToken.Unrecognized(2, "c"),
                });

            // Whitespace tokens
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
                "\n",
                new[] { PkgdefToken.Whitespace(0, "\n") });
            CreateTest(
                "\r\n",
                new[] { PkgdefToken.Whitespace(0, "\r\n") });

            // Line comment
            CreateTest(
                "/",
                new[] { PkgdefToken.Unrecognized(0, "/") },
                new[] { PkgdefIssue.Error(0, 1, "Missing the line-comment's second forward slash ('/').") });
            CreateTest(
                "/ ",
                new[]
                {
                    PkgdefToken.Unrecognized(0, "/"),
                    PkgdefToken.Whitespace(1, " "),
                },
                new[] { PkgdefIssue.Error(1, 1, "Expected the line-comment's second forward slash ('/').") });
            CreateTest(
                "//",
                new[] { PkgdefToken.LineComment(0, "//") });
            CreateTest(
                "// hello",
                new[] { PkgdefToken.LineComment(0, "// hello") });
            CreateTest(
                "// hello\r",
                new[] { PkgdefToken.LineComment(0, "// hello\r") });
            CreateTest(
                "// hello there\n",
                new[] { PkgdefToken.LineComment(0, "// hello there\n") });
            CreateTest(
                "// hello there\r\n",
                new[] { PkgdefToken.LineComment(0, "// hello there\r\n") });
            CreateTest(
                "// hello there\n   ",
                new[]
                {
                    PkgdefToken.LineComment(0, "// hello there\n"),
                    PkgdefToken.Whitespace(15, "   "),
                });
            CreateTest(
                "// hello there\r\n   ",
                new[]
                {
                    PkgdefToken.LineComment(0, "// hello there\r\n"),
                    PkgdefToken.Whitespace(16, "   "),
                });
            CreateTest(
                "//a\n//b\n//c",
                new[]
                {
                    PkgdefToken.LineComment(0, "//a\n"),
                    PkgdefToken.LineComment(4, "//b\n"),
                    PkgdefToken.LineComment(8, "//c"),
                });

            // Quoted strings
            CreateTest(
                "\"",
                new[] { PkgdefToken.QuotedString(0, "\"") },
                new[] { PkgdefIssue.Error(0, 1, "Missing quoted-string's closing double-quote ('\"').") });
            CreateTest(
                "\"abc",
                new[] { PkgdefToken.QuotedString(0, "\"abc") },
                new[] { PkgdefIssue.Error(0, 4, "Missing quoted-string's closing double-quote ('\"').") });
            CreateTest(
                "\"abc\n",
                new[]
                {
                    PkgdefToken.QuotedString(0, "\"abc"),
                    PkgdefToken.Whitespace(4, "\n"),
                },
                new[] { PkgdefIssue.Error(0, 4, "Missing quoted-string's closing double-quote ('\"').") });
            CreateTest(
                "\"abc\"",
                new[] { PkgdefToken.QuotedString(0, "\"abc\"") });
            CreateTest(
                "\"abc\"\"d\"",
                new[]
                {
                    PkgdefToken.QuotedString(0, "\"abc\""),
                    PkgdefToken.QuotedString(5, "\"d\""),
                });

            // At symbol
            CreateTest(
                "@",
                new[] { PkgdefToken.AtSign(0) });
        }
    }
}
