using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pkgdef_CSharp;

namespace Pkgdef_CSharp_Tests
{
    [TestClass]
    public class CurrentIndexIteratorTests
    {
        [TestMethod]
        public void Create_WithNullIEnumerator()
        {
            Assert.ThrowsException<PreConditionException>(() => CurrentIndexIterator.Create((IEnumerator<char>)null));
        }

        [TestMethod]
        public void Create_WithNonStartedIEnumerator()
        {
            string characters = "hello";
            IEnumerator<char> enumerator = characters.GetEnumerator();
            CurrentIndexIterator<char> iterator = CurrentIndexIterator.Create(enumerator);
            Assert.IsNotNull(iterator);
            Assert.IsFalse(iterator.HasStarted());
            Assert.IsFalse(iterator.HasCurrent());
            AssertEx.Throws(() => iterator.GetCurrentIndex(),
                new PreConditionException("this.HasCurrent() cannot be false."));

            int expectedCurrentIndex = 0;
            foreach (char character in characters)
            {
                Assert.IsTrue(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsTrue(iterator.HasCurrent());
                Assert.AreEqual(character, iterator.Current);
                Assert.AreEqual(character, enumerator.Current);
                Assert.AreEqual(expectedCurrentIndex, iterator.GetCurrentIndex());
                expectedCurrentIndex++;
            }

            for (int i = 0; i < 2; ++i)
            {
                Assert.IsFalse(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsFalse(iterator.HasCurrent());
                Assert.ThrowsException<PreConditionException>(() => iterator.Current);
                Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
                AssertEx.Throws(() => iterator.GetCurrentIndex(),
                    new PreConditionException("this.HasCurrent() cannot be false."));
            }
        }

        [TestMethod]
        public void Create_WithStartedIEnumerator()
        {
            string characters = "hello";
            IEnumerator<char> enumerator = characters.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());

            CurrentIndexIterator<char> iterator = CurrentIndexIterator.Create(enumerator);
            Assert.IsNotNull(iterator);
            Assert.IsTrue(iterator.HasStarted());
            Assert.IsTrue(iterator.HasCurrent());
            Assert.AreEqual('h', iterator.Current);
            Assert.AreEqual('h', enumerator.Current);

            int expectedCurrentIndex = 0;
            Assert.AreEqual(expectedCurrentIndex, iterator.GetCurrentIndex());
            expectedCurrentIndex++;

            foreach (char character in characters.Skip(1))
            {
                Assert.IsTrue(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsTrue(iterator.HasCurrent());
                Assert.AreEqual(character, iterator.Current);
                Assert.AreEqual(character, enumerator.Current);
                Assert.AreEqual(expectedCurrentIndex, iterator.GetCurrentIndex());
                expectedCurrentIndex++;
            }

            for (int i = 0; i < 2; ++i)
            {
                Assert.IsFalse(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsFalse(iterator.HasCurrent());
                Assert.ThrowsException<PreConditionException>(() => iterator.Current);
                Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
                AssertEx.Throws(() => iterator.GetCurrentIndex(),
                    new PreConditionException("this.HasCurrent() cannot be false."));
            }
        }

        [TestMethod]
        public void Create_WithFinishedIEnumerator()
        {
            string characters = "";
            IEnumerator<char> enumerator = characters.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());

            CurrentIndexIterator<char> iterator = CurrentIndexIterator.Create(enumerator);
            Assert.IsNotNull(iterator);
            Assert.IsTrue(iterator.HasStarted());
            Assert.IsFalse(iterator.HasCurrent());
            Assert.ThrowsException<PreConditionException>(() => iterator.Current);
            Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
            AssertEx.Throws(() => iterator.GetCurrentIndex(),
                new PreConditionException("this.HasCurrent() cannot be false."));

            for (int i = 0; i < 2; ++i)
            {
                Assert.IsFalse(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsFalse(iterator.HasCurrent());
                Assert.ThrowsException<PreConditionException>(() => iterator.Current);
                Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
                AssertEx.Throws(() => iterator.GetCurrentIndex(),
                    new PreConditionException("this.HasCurrent() cannot be false."));
            }
        }

        [TestMethod]
        public void Create_WithNullIEnumerable()
        {
            Assert.ThrowsException<PreConditionException>(() => CurrentIndexIterator.Create((IEnumerable<char>)null));
        }

        [TestMethod]
        public void Create_WithEmptyIEnumerable()
        {
            CurrentIndexIterator<char> iterator = CurrentIndexIterator.Create("");
            Assert.IsFalse(iterator.HasStarted());
            Assert.IsFalse(iterator.HasCurrent());
            AssertEx.Throws(() => iterator.GetCurrentIndex(),
                new PreConditionException("this.HasCurrent() cannot be false."));

            for (int i = 0; i < 2; ++i)
            {
                Assert.IsFalse(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsFalse(iterator.HasCurrent());
                AssertEx.Throws(() => iterator.GetCurrentIndex(),
                    new PreConditionException("this.HasCurrent() cannot be false."));
            }
        }

        [TestMethod]
        public void Create_WithNonEmptyIEnumerable()
        {
            string characters = "hello";
            CurrentIndexIterator<char> iterator = CurrentIndexIterator.Create(characters);
            Assert.IsNotNull(iterator);
            Assert.IsFalse(iterator.HasStarted());
            Assert.IsFalse(iterator.HasCurrent());
            AssertEx.Throws(() => iterator.GetCurrentIndex(),
                new PreConditionException("this.HasCurrent() cannot be false."));

            int expectedCurrentIndex = 0;
            foreach (char character in characters)
            {
                Assert.IsTrue(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsTrue(iterator.HasCurrent());
                Assert.AreEqual(character, iterator.Current);
                Assert.AreEqual(expectedCurrentIndex, iterator.GetCurrentIndex());
                expectedCurrentIndex++;
            }

            for (int i = 0; i < 2; ++i)
            {
                Assert.IsFalse(iterator.Next());
                Assert.IsTrue(iterator.HasStarted());
                Assert.IsFalse(iterator.HasCurrent());
                Assert.ThrowsException<PreConditionException>(() => iterator.Current);
                AssertEx.Throws(() => iterator.GetCurrentIndex(),
                    new PreConditionException("this.HasCurrent() cannot be false."));
            }
        }
    }
}
