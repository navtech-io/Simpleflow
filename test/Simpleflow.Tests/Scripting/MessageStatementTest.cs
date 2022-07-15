// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.CodeGenerator;
using Simpleflow.Exceptions;
using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Scripting
{
    public class MessageStatementTest
    {
        [Fact]
        public void MessageWithStringLiteral()
        {
            // Arrange
            var script =
                @"
                    message /*text*/ ""Success""
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());
            
            // Assert
            Assert.Equal(actual: output.Messages.Count, expected: 1);
            Assert.Equal(actual: output.Messages[0], expected: "Success");
        }

        [Fact] 
        public void MessageWithIdentifier()
        {
            // Arrange
            var context = new SampleArgument() {Id = 10};
            var script =
                @"
                    let success = ""Success""
                    let num = 2

                    message success
                    message num
                    message arg.Id
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, context);
            
            // Assert
            Assert.Equal(actual: output.Messages.Count, expected: 3);
            Assert.Equal(actual: output.Messages[0], expected: "Success");
            Assert.Equal(actual: output.Messages[1], expected: "2");
            Assert.Equal(actual: output.Messages[2], expected: "10");
        }

        [Fact]
        public void MessageWithUndeclaredIdentifier()
        {
            // Arrange
            var script =
                @"
                    message success
                ";

            // Act  & Assert
            Assert.Throws<UndeclaredVariableException>(() => SimpleflowEngine.Run(script, new object()));
        }

        [Fact]
        public void MessageWithUndeclaredProperty()
        {
            // Arrange
            var script =
                @"
                    message arg.name
                ";

            // Act  & Assert
            Assert.Throws<InvalidPropertyException>(() => SimpleflowEngine.Run(script, new object()));
        }

        [Fact]
        public void MessageWithSyntaxError()
        {
            // Arrange
            var script =
                @"
                    message xyz arg.name
                ";

            // Act  & Assert
            Assert.Throws<SyntaxException>(() => SimpleflowEngine.Run(script, new object()));
        }
    }
}
