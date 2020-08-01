using System;
using System.Linq;
using System.Text;
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
                        AssertEx.AreEqual(expectedSegments, document.GetSegments());
                    }

                    if (expectedIssues == null)
                    {
                        Assert.AreEqual(0, document.GetIssues().Count);
                    }
                    else
                    {
                        AssertEx.AreEqual(expectedIssues, document.GetIssues());
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

            ParseTest(
                new StringBuilder()
                    .AppendLine("//=================================================================================================")
                    .AppendLine("// Build")
                    .AppendLine("//=================================================================================================")
                    .AppendLine()
                    .ToString(),
                new[]
                {
                    PkgdefSegment.LineComment(0, "//================================================================================================="),
                    PkgdefSegment.NewLine(99, "\r\n"),
                    PkgdefSegment.LineComment(101, "// Build"),
                    PkgdefSegment.NewLine(109, "\r\n"),
                    PkgdefSegment.LineComment(111, "//================================================================================================="),
                    PkgdefSegment.NewLine(210, "\r\n"),
                    PkgdefSegment.NewLine(212, "\r\n"),
                });

            //ParseTest(
            //    new StringBuilder()
            //        .AppendLine("//=================================================================================================")
            //        .AppendLine("// Build")
            //        .AppendLine("//=================================================================================================")
            //        .AppendLine()
            //        .AppendLine("[$RootKey$\\Editors\\{19D8ED1B-FFD3-4DFA-B329-E47AD8752E9E}]")
            //        .AppendLine("@=\"RequestEditorFactory\"")
            //        .AppendLine("\"DisplayName\"=\"#108\"")
            //        .AppendLine("\"DeferUntilIntellisenseIsReady\"=dword:00000000")
            //        .AppendLine("\"Package\"=\"{739f34b3-9ba6-4356-9178-ac3ea81bdf47}\"")
            //        .AppendLine("\"CommonPhysicalViewAttributes\"=dword:3")
            //        .ToString(),
            //    new[]
            //    {
            //        PkgdefSegment.LineComment(0, "//================================================================================================="),
            //        PkgdefSegment.NewLine(50, "\r\n"),
            //        PkgdefSegment.LineComment(70, "// Build"),
            //        PkgdefSegment.NewLine(75, "\r\n"),
            //        PkgdefSegment.LineComment(80, "//================================================================================================="),
            //        PkgdefSegment.NewLine(85, "\r\n"),
            //    });
        }
    }
}
