using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pkgdef_CSharp;

namespace Pkgdef_CSharp_Tests
{
    /// <summary>
    /// A collection of assertion methods.
    /// </summary>
    internal static class AssertEx
    {
        /// <summary>
        /// Invoke the provided action with the expectation that it will thrown an exception.
        /// Assert that the thrown exception matches the provided expected exception.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <param name="expectedException">The expected exception. If this is null, then no
        /// exception validation will be performed and the actual exception will just be returned.</param>
        /// <returns></returns>
        public static Exception Throws(Action action)
        {
            return AssertEx.Throws<Exception>(action);
        }

        /// <summary>
        /// Invoke the provided action with the expectation that it will thrown an exception.
        /// Assert that the thrown exception matches the provided expected exception.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <param name="expectedException">The expected exception. If this is null, then no
        /// exception validation will be performed and the actual exception will just be returned.</param>
        /// <returns></returns>
        public static T Throws<T>(Action action, T expectedException = null) where T : Exception
        {
            PreCondition.AssertNotNull(action, nameof(action));

            Exception actualException = null;
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                actualException = e;
            }

            if (actualException == null)
            {
                if (expectedException == null)
                {
                    Assert.Fail("Expected an exception to be thrown.");
                }
                else
                {
                    Assert.Fail($"Expected an exception of type {expectedException.GetType()} to be thrown, but no exception was thrown.");
                }
            }
            else if (expectedException != null)
            {
                Assert.AreEqual(expectedException.GetType(), actualException.GetType(), "Wrong exception type thrown.");
                Assert.AreEqual(expectedException.Message, actualException.Message, "Wrong exception message.");
            }

            return actualException as T;
        }
    }
}
