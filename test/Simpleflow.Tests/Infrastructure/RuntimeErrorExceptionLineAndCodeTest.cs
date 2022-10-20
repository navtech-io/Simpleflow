// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using Simpleflow.Exceptions;
using Xunit;

using static Simpleflow.Tests.Helpers.TestsHelper;

namespace Simpleflow.Tests.Infrastructure
{
    public class RuntimeErrorExceptionLineAndCodeTest
    {
        [Fact]
        public void ErrorLineNumber()
        {
            // Arrange

            var script =
                @"
                    let x = 10    
                    error ""test""
                        
                    message 2 / 0

                    output x
                ";

            // Act 
            var exception = Try(() => SimpleflowEngine.Run(script, new object()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<SimpleflowRuntimeException>(exception);
            Assert.Equal(5, ((SimpleflowRuntimeException)exception).LineNumber);

        }

        [Fact]
        public void ErrorLineNumberAtRuleStatement()
        {
            // Arrange

            var script =
                @"
                    let x = 10    
                    error ""test""
                        
                    rule when 1 == 1 then

                        message 2 / 0

                    output x
                ";

            // Act 
            var exception = Try(() => SimpleflowEngine.Run(script, new object()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<SimpleflowRuntimeException>(exception);

            var sre = ((SimpleflowRuntimeException)exception);
            Assert.Equal(7, sre.LineNumber);
            Assert.Equal("                        message 2 / 0\r", sre.Statement);

        }
    }
}
