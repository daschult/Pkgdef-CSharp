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
            void ParseTest(string text, PkgdefToken[] expectedTokens = null, Exception expectedException = null)
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

                    if (expectedTokens == null)
                    {
                        Assert.AreEqual(0, document.GetTokens().Count);
                    }
                    else
                    {
                        CollectionAssert.AreEqual(expectedTokens, document.GetTokens().ToArray());
                    }
                }
            }

            ParseTest(
                null,
                expectedException: new PreConditionException("text cannot be null."));
            ParseTest("");
            ParseTest(
                "   ",
                new[] { PkgdefToken.Whitespace(0, "   ") });
            ParseTest(
                "a b",
                new[]
                {
                    PkgdefToken.Unrecognized(0, "a"),
                    PkgdefToken.Whitespace(1, " "),
                    PkgdefToken.Unrecognized(2, "b"),
                });
            ParseTest(
                "[",
                new[] { PkgdefToken.Unrecognized(0, "[") });
        }
    }
}
