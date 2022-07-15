// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.Exceptions;
using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Scripting
{
    public class OutputStatementTest
    {
        [Fact]
        public void Output()
        {
            // Arrange
            var arg = new SampleArgument();
            var script =
                @"
                    let value = 12
                    let text  = ""test""

                    output value
                    output text
                    output arg

                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, arg);
            
            // Assert
            Assert.Equal(actual: output.Output.Count, expected: 3);

            Assert.Equal(actual: output.Output["value"], expected: 12);
            Assert.Equal(actual: output.Output["text"], expected: "test");
            Assert.Equal(actual: output.Output["arg"], expected: arg);

            
        }


        [Fact]
        public void OutputWithUndeclaredProperty()
        {
            // Arrange
            var script =
                @"
                    output xyz
                ";

            // Act  & Assert
            Assert.Throws<UndeclaredVariableException>(() => SimpleflowEngine.Run(script, new object()));
        }

        [Fact]
        public void OutputWithSyntaxError()
        {
            // Arrange
            var script =
                @"
                    output ""test""
                ";

            // Act  & Assert
            Assert.Throws<SyntaxException>(() => SimpleflowEngine.Run(script, new object()));
        }
    }
}
