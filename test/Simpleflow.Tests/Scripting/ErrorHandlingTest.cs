// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class ErrorHandlingTest
    {
        [Fact]
        public void SimpleflowAstWithErrors()
        {
            // Arrange
            var flowScript =
            @" 
                let x, err = 2 / 0
                
                output err

                error `got an error - {err.Message}`

                set x, err2 = 5 + 3

                rule when err2 then
                    message ""No error""
            ";

            // Act    
            var result = SimpleflowEngine.Run(flowScript, new object());

            // Assert
            Assert.IsType<DivideByZeroException>(result.Output["err"]);
            Assert.Equal("got an error - Attempted to divide by zero.", result.Errors[0]);
            Assert.Equal("No error", result.Messages[0]);

        }

    }
}
