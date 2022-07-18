using System;
using Xunit;

namespace Simpleflow.Tests
{
    internal static class AssertEx
    {
        public static void Throws<T>(string message, Action code)
        {
            bool shouldThrow = false;
            try
            {
                code();
            }
            catch (Exception ex)
            {
                Assert.IsType<T>(ex);
                Assert.Equal(message, ex.Message);
                shouldThrow = true;
            }
            Assert.True(shouldThrow, "Not Thrown");
        }

        public static void Throws<T>(Action<T> assertException, Action code) where T : System.Exception
        {
            bool shouldThrow = false; 
            try
            {
                code();
            }
            catch (Exception ex)
            {
                Assert.IsType<T>(ex);
                assertException(ex as T);
                shouldThrow = true;
            }

            Assert.True(shouldThrow, "Not Thrown");
            
        }
    }
}
