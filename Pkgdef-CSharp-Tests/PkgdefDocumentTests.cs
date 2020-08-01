using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pkgdef_CSharp;

namespace Pkgdef_CSharp_Tests
{
    [TestClass]
    public class PkgdefDocumentTests
    {
        [TestMethod]
        public void Parse()
        {
            void ParseTest(string text, PkgdefSegment[] expectedSegments = null, PkgdefIssue[] expectedIssues = null, Exception expectedException = null)
            {
                if (expectedException != null)
                {
                    AssertEx.Throws(() => PkgdefDocument.Parse(text), expectedException);
                }
                else
                {
                    PkgdefDocument document = PkgdefDocument.Parse(text);
                    Assert.IsNotNull(document);
                    Assert.AreEqual(text, document.GetText());

                    if (expectedSegments == null)
                    {
                        Assert.AreEqual(0, document.GetSegments().Count);
                    }
                    else
                    {
                        CollectionAssert.AreEqual(expectedSegments, document.GetSegments().ToArray());
                    }

                    if (expectedIssues == null)
                    {
                        Assert.AreEqual(0, document.GetIssues().Count);
                    }
                    else
                    {
                        CollectionAssert.AreEqual(expectedIssues, document.GetIssues().ToArray());
                    }
                }
            }

            ParseTest(
                null,
                expectedException: new PreConditionException("text cannot be null."));
            ParseTest(
                "",
                new PkgdefSegment[0]);

            ParseTest(
                " ",
                new[] { PkgdefSegment.Whitespace(0, " ") });
            ParseTest(
                "    ",
                new[] { PkgdefSegment.Whitespace(0, "    ") });
            ParseTest(
                "\t",
                new[] { PkgdefSegment.Whitespace(0, "\t") });
            ParseTest(
                "\t\t\t",
                new[] { PkgdefSegment.Whitespace(0, "\t\t\t") });
            ParseTest(
                " \t \t \t ",
                new[] { PkgdefSegment.Whitespace(0, " \t \t \t ") });
            ParseTest(
                "\r",
                new[] { PkgdefSegment.Whitespace(0, "\r") });

            ParseTest(
                "\n",
                new[] { PkgdefSegment.NewLine(0, "\n") });
            ParseTest(
                "\r\n",
                new[] { PkgdefSegment.NewLine(0, "\r\n") });

            ParseTest(
                "/",
                new[] { PkgdefSegment.Unrecognized(0, "/") },
                new[] { PkgdefIssue.Error(0, 1, "Missing line-comment's second forward slash ('/').") });
            ParseTest(
                "/ ",
                new[]
                {
                    PkgdefSegment.Unrecognized(0, "/"),
                    PkgdefSegment.Whitespace(1, " "),
                },
                new[] { PkgdefIssue.Error(1, 1, "Expected line-comment's second forward slash ('/').") });
            ParseTest(
                "//",
                new[] { PkgdefSegment.LineComment(0, "//") });
            ParseTest(
                "// hello",
                new[] { PkgdefSegment.LineComment(0, "// hello") });
            ParseTest(
                "// hello\r",
                new[] { PkgdefSegment.LineComment(0, "// hello\r") });
            ParseTest(
                "// hello there\n",
                new[]
                {
                    PkgdefSegment.LineComment(0, "// hello there"),
                    PkgdefSegment.NewLine(14, "\n"),
                });
            ParseTest(
                "// hello there\r\n",
                new[]
                {
                    PkgdefSegment.LineComment(0, "// hello there"),
                    PkgdefSegment.NewLine(14, "\r\n"),
                });
            ParseTest(
                "// hello there\n   ",
                new[]
                {
                    PkgdefSegment.LineComment(0, "// hello there"),
                    PkgdefSegment.NewLine(14, "\n"),
                    PkgdefSegment.Whitespace(15, "   "),
                });
            ParseTest(
                "// hello there\r\n   ",
                new[]
                {
                    PkgdefSegment.LineComment(0, "// hello there"),
                    PkgdefSegment.NewLine(14, "\r\n"),
                    PkgdefSegment.Whitespace(16, "   "),
                });
            ParseTest(
                "//a\n//b\n//c",
                new[]
                {
                    PkgdefSegment.LineComment(0, "//a"),
                    PkgdefSegment.NewLine(3, "\n"),
                    PkgdefSegment.LineComment(4, "//b"),
                    PkgdefSegment.NewLine(7, "\n"),
                    PkgdefSegment.LineComment(8, "//c"),
                });
        }
    }
}
