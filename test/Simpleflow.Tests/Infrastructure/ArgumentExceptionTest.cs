using System;
using Xunit;

namespace Simpleflow.Tests.Infrastructure
{
    public class ArgumentExceptionTest
    {
        [Fact]
        public void ThrowIfNull()
        {
             Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNull(null, string.Empty));
        }

        [Fact]
        public void ThrowIfNullWithParamName()
        {
            string name = null;
            Assert.Throws<ArgumentNullException>("name", () => ArgumentException.ThrowIfNull(name));
        }

        [Fact]
        public void ThrowIfNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrEmpty(String.Empty, string.Empty));
        }

        [Fact]
        public void ThrowIfNullOrEmptyWithParamName()
        {
            string name = String.Empty;
            Assert.Throws<ArgumentNullException>("name", () => ArgumentException.ThrowIfNullOrEmpty(name));
        }
    }
}
