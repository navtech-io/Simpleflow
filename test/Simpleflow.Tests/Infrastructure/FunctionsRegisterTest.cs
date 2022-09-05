// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

using Simpleflow.Exceptions;

namespace Simpleflow.Tests
{
    public class FunctionsRegisterTest
    {
        [Fact]
        public void AddFunction()
        {
            // Arrange
            var functionsConfig = new FunctionRegister();
            functionsConfig.Add("test4",    (Action<int>)TestActivityMethod);

            // Act
            var test4Activity = functionsConfig.GetFunction("test4", null);

            // Assert
            Assert.Equal(TestActivityMethod, (Action<int>)test4Activity.Reference);

            // test
            test4Activity.Reference.DynamicInvoke(2);
        }

        [Fact]
        public void AddDuplicateFunction()
        {
            // Arrange
            var functionsConfig = new FunctionRegister();
            functionsConfig.Add("test4", (Action<int>)TestActivityMethod);


            // Act & Assert
            AssertEx.Throws<DuplicateFunctionException>(
                ex => Assert.Equal("test4", ex.FunctionName),
                () => functionsConfig.Add("test4", (Action<int>)TestActivityMethod1));

        }

        [Fact]
        public void CheckInvalidFunctionNameException()
        {
            // Arrange
            var functionsConfig = new FunctionRegister();
            
            // Act & Assert
            AssertEx.Throws<InvalidFunctionNameException>(
                ex => Assert.Equal("test#@!3", ex.FunctionName),
                () => functionsConfig.Add("test#@!3", (Action<int>)TestActivityMethod));
        }

        private static void TestActivityMethod(int a)
        {
            // dummy method to run above testing...
        }
        private static void TestActivityMethod1(int a)
        {
            // dummy method to run above testing...
        }
    }
}

