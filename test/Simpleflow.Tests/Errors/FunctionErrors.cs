// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.Tests.Helpers;
using Simpleflow.Exceptions;

namespace Simpleflow.Tests.Errors
{
    public class FunctionErrors
    {
        [Fact]
        public void ParameterTypeMismatch()
        {
            // Arrange
            var arg = new SampleArgument();
            var script =
                @"
                    let date = $GetCurrentDateTime ( timezone: 1 )
                    output date 
                ";

            // Act & Assert
            Assert.Throws<ValueTypeMismatchException>(() => SimpleflowEngine.Run(script, arg));
        }

        [Fact]
        public void InvalidFunctionParameterName()
        {
            // Arrange
            var arg = new SampleArgument();
            var script =
                @"
                    let date = $GetCurrentDateTime ( test: 1 )
                    output date 
                ";

            // Act & Assert
            Assert.Throws<InvalidFunctionParameterNameException>(() => SimpleflowEngine.Run(script, arg));
        }

        [Fact]
        public void InvalidFunctionParameterValue()
        {
            // Arrange
            var arg = new SampleArgument();
            var script =
                @"
                    let date = $GetCurrentDateTime ( timezone: 1 )
                    output date 
                ";

            // Act & Assert
            Assert.Throws<ValueTypeMismatchException>(() => SimpleflowEngine.Run(script, arg));
        }


        [Fact]
        public void DuplicateFunctionParameters()
        {
            // Arrange
            var arg = new SampleArgument();
            var script =
                @"
                    let date = $GetCurrentDateTime ( timezone: ""test"", timezone: ""test2"" )
                    output date 
                ";

            // Act & Assert
            Assert.Throws<DuplicateParametersException>(() => SimpleflowEngine.Run(script, arg));
        }

        [Fact]
        public void InvalidFunctionName()
        {
            // Arrange
            var context = new SampleArgument();
            var script =
                @"
                    let date = $GetDateNeverWork_____()
                    message date 
                ";

            // Act & Assert
            Assert.Throws<InvalidFunctionException>(() => SimpleflowEngine.Run(script, context));

        }
    }
}
