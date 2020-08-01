using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pkgdef_CSharp;

namespace Pkgdef_CSharp_Tests
{
    [TestClass]
    public class AssertExTests
    {
        [TestMethod]
        public void Throws_WithNullAction()
        {
            PreConditionException exception = Assert.ThrowsException<PreConditionException>(() => AssertEx.Throws(null));
            Assert.AreEqual("action cannot be null.", exception.Message);
        }

        [TestMethod]
        public void Throws_WithActionThatDoesntThrowAndNoExpectedException()
        {
            AssertFailedException exception = Assert.ThrowsException<AssertFailedException>(() => AssertEx.Throws(() => {}));
            Assert.AreEqual("Assert.Fail failed. Expected an exception to be thrown.", exception.Message);
        }

        [TestMethod]
        public void Throws_WithActionThatDoesntThrowAndExpectedException()
        {
            AssertFailedException exception = Assert.ThrowsException<AssertFailedException>(() => AssertEx.Throws(() => { }, new PreConditionException("blah")));
            Assert.AreEqual("Assert.Fail failed. Expected an exception of type Pkgdef_CSharp.PreConditionException to be thrown, but no exception was thrown.", exception.Message);
        }

        [TestMethod]
        public void Throws_WithActionThatThrowsAndNoExpectedException()
        {
            Exception exception = AssertEx.Throws(() => { throw new ArgumentException("fake-message"); });
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
            Assert.AreEqual("fake-message", exception.Message);
        }

        [TestMethod]
        public void Throws_WithActionThatThrowsAndDifferentTypeExpectedException()
        {
            AssertFailedException exception = Assert.ThrowsException<AssertFailedException>(() => AssertEx.Throws(() => { throw new ArgumentException("fake-message"); }, new PreConditionException("blah")));
            Assert.IsNotNull(exception);
            Assert.AreEqual("Assert.AreEqual failed. Expected:<Pkgdef_CSharp.PreConditionException>. Actual:<System.ArgumentException>. Wrong exception type thrown.", exception.Message);
        }

        [TestMethod]
        public void Throws_WithActionThatThrowsAndDifferentMessageExpectedException()
        {
            AssertFailedException exception = Assert.ThrowsException<AssertFailedException>(() => AssertEx.Throws(() => { throw new ArgumentException("fake-message"); }, new ArgumentException("blah")));
            Assert.IsNotNull(exception);
            Assert.AreEqual("Assert.AreEqual failed. Expected:<blah>. Actual:<fake-message>. Wrong exception message.", exception.Message);
        }

        [TestMethod]
        public void Throws_WithActionThatThrowsExpectedException()
        {
            Exception exception = AssertEx.Throws(() => { throw new ArgumentException("fake-message"); }, new ArgumentException("fake-message"));
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
            Assert.AreEqual("fake-message", exception.Message);
        }

        [TestMethod]
        public void AreEqual_IEnumerable_WithNullAndNull()
        {
            AssertEx.AreEqual((IEnumerable<int>)null, (IEnumerable<int>)null);
        }

        [TestMethod]
        public void AreEqual_IEnumerable_WithNullAndEmpty()
        {
            AssertEx.Throws(() => AssertEx.AreEqual((IEnumerable<int>)null, new int[0]),
                new AssertFailedException("Assert.Fail failed. Expected the value to be null."));
        }

        [TestMethod]
        public void AreEqual_IEnumerable_WithEmptyAndNull()
        {
            AssertEx.Throws(() => AssertEx.AreEqual(new int[0], (IEnumerable<int>)null),
                new AssertFailedException("Assert.Fail failed. Expected the value to be non-null."));
        }

        [TestMethod]
        public void AreEqual_IEnumerable_WithEmptyAndEmpty()
        {
            AssertEx.AreEqual(new int[0], new int[0]);
        }

        [TestMethod]
        public void AreEqual_IEnumerable_WithDifferentLengths()
        {
            AssertEx.Throws(() => AssertEx.AreEqual(new int[] { 0, 1, 2 }, new int[] { 0 }),
                new AssertFailedException("Assert.AreEqual failed. Expected:<3>. Actual:<1>. Element counts are not equal."));
        }

        [TestMethod]
        public void AreEqual_IEnumerable_WithDifferentElements()
        {
            AssertEx.Throws(() => AssertEx.AreEqual(new int[] { 0, 1, 2 }, new int[] { 0, 3, 2 }),
                new AssertFailedException("Assert.AreEqual failed. Expected:<1>. Actual:<3>. Values at index 1 are not equal."));
        }

        [TestMethod]
        public void AreEqual_IEnumerable_WithEqualElements()
        {
            AssertEx.AreEqual(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 });
        }
    }
}
