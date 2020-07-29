using System;
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
    }
}
