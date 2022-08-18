// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.Exceptions;
using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Scripting
{
    public class ErrorStatementTest
    {
        [Fact]
        public void ErrorWithStringLiteral()
        {
            // Arrange
            var script =
                @"
                    error ""Invalid""
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());
            
            // Assert
            Assert.Equal(actual: output.Errors.Count, expected: 1);
            Assert.Equal(actual: output.Errors[0], expected: "Invalid");
        }

        [Fact] 
        public void ErrorWithIdentifier()
        {
            // Arrange
            var context = new SampleArgument() {Id = 10};
            var script =
                @"
                    let err = ""Error""
                    let num = 2

                    error err
                    error num
                    error arg.Id
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, context);
            
            // Assert
            Assert.Equal(actual: output.Errors.Count, expected: 3);
            Assert.Equal(actual: output.Errors[0], expected: "Error");
            Assert.Equal(actual: output.Errors[1], expected: "2");
            Assert.Equal(actual: output.Errors[2], expected: "10");
        }

        [Fact]
        public void ErrorWithUndeclaredIdentifier()
        {
            // Arrange
            var script =
                @"
                    error invalid
                ";

            // Act  & Assert
            Assert.Throws<UndeclaredVariableException>(() => SimpleflowEngine.Run(script, new object()));
        }

        [Fact]
        public void ErrorWithUndeclaredProperty()
        {
            // Arrange
            var script =
                @"
                    error arg.name
                ";

            // Act  & Assert
            Assert.Throws<InvalidPropertyException>(() => SimpleflowEngine.Run(script, new object()));
        }

        [Fact]
        public void ErrorWithSyntaxError()
        {
            // Arrange
            var script =
                @"
                    error xyz arg.name
                ";

            // Act  & Assert
            Assert.Throws<SyntaxException>(() => SimpleflowEngine.Run(script, new object()));
        }

        [Fact]
        public void ErrorWithExpression()
        {
            // Arrange
            var context = new SampleArgument() { Id = 10 };

            var script =
                @"
                    let value  = 2 * 3
                    error value * 2 + 2
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run<SampleArgument>(script, context);

            // Assert
            Assert.Equal("14", output.Errors[0]);
        }
    }
}
