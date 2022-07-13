// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

namespace Simpleflow.Tests
{
    public class FunctionsConfigTest
    {
        [Fact]
        public void AddFunction()
        {
            // Arrange
            var functionsConfig = new FunctionRegister();
            functionsConfig.Add("test4",    (Action<int>)TestActivityMethod);

            // Act
            var test4Activity = functionsConfig.GetFunction("test4");

            // Assert
            Assert.Equal(TestActivityMethod, (Action<int>)test4Activity);

            // test
            test4Activity.DynamicInvoke(2);
        }

        private static void TestActivityMethod(int a)
        {
            // dummy method to run above testing...
        }
    }
}

