using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pkgdef_CSharp;

namespace Pkgdef_CSharp_Tests
{
    [TestClass]
    public class PkgdefTokenTests
    {
        [TestMethod]
        public void Constructor()
        {
            void ConstructorTest(int startIndex, string text, PkgdefTokenType tokenType, Exception expectedException = null)
            {
                if (expectedException != null)
                {
                    AssertEx.Throws(() => new PkgdefToken(startIndex, text, tokenType), expectedException);
                }
                else
                {
                    PkgdefToken token = new PkgdefToken(startIndex, text, tokenType);
                    Assert.AreEqual(startIndex, token.GetStartIndex());
                    Assert.AreEqual(text, token.GetText());
                    Assert.AreEqual(tokenType, token.GetTokenType());
                    Assert.AreEqual(text.Length, token.GetLength());
                    Assert.AreEqual(startIndex + text.Length, token.GetAfterEndIndex());
                    Assert.AreEqual(startIndex + text.Length - 1, token.GetEndIndex());
                }
            }

            ConstructorTest(-1, "hello", PkgdefTokenType.Unrecognized, expectedException: new PreConditionException("startIndex (-1) must be greater than or equal to 0."));
            ConstructorTest(0, null, PkgdefTokenType.Unrecognized, expectedException: new PreConditionException("text cannot be null."));
            ConstructorTest(0, "", PkgdefTokenType.Unrecognized, expectedException: new PreConditionException("text cannot be empty."));
            ConstructorTest(0, "abc", PkgdefTokenType.Unrecognized);
        }

        [TestMethod]
        public void LineComment()
        {
            PkgdefToken token = PkgdefToken.LineComment(1, "// hello");
            Assert.AreEqual(1, token.GetStartIndex());
            Assert.AreEqual("// hello", token.GetText());
            Assert.AreEqual(PkgdefTokenType.LineComment, token.GetTokenType());
        }

        [TestMethod]
        public void Whitespace()
        {
            PkgdefToken token = PkgdefToken.Whitespace(1, "  \t");
            Assert.AreEqual(1, token.GetStartIndex());
            Assert.AreEqual("  \t", token.GetText());
            Assert.AreEqual(PkgdefTokenType.Whitespace, token.GetTokenType());
        }

        [TestMethod]
        public void QuotedString()
        {
            PkgdefToken token = PkgdefToken.QuotedString(1, "\"hello\"");
            Assert.AreEqual(1, token.GetStartIndex());
            Assert.AreEqual("\"hello\"", token.GetText());
            Assert.AreEqual(PkgdefTokenType.QuotedString, token.GetTokenType());
        }

        [TestMethod]
        public void Unrecognized()
        {
            PkgdefToken token = PkgdefToken.Unrecognized(1, "abc");
            Assert.AreEqual(1, token.GetStartIndex());
            Assert.AreEqual("abc", token.GetText());
            Assert.AreEqual(PkgdefTokenType.Unrecognized, token.GetTokenType());
        }
    }
}
