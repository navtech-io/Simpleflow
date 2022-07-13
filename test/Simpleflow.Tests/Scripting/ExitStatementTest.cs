// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Scripting
{
    public class ExitStatementTest
    {
        [Theory]
        [InlineData("true", 1)]
        [InlineData("false", 2)]
        public void ExitStatement(string exit, int expectedMessages)
        {
            // Arrange
            
            var script =
                @$"
                  let exitBefore = {exit}  
                  message ""before-exit""

                  rule when exitBefore then  
                       exit
                  end rule

                  message ""after-exit""
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.Equal(actual: output.Messages.Count, expected: expectedMessages);
            Assert.Equal(actual: output.Messages[0], expected: "before-exit");

            if (expectedMessages == 2) Assert.Equal(actual: output.Messages[1], expected: "after-exit");
        }

        [Fact]
        public void OnlyExitStatement()
        {
            // Arrange

            var script =
                @$"
                  exit
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new object());

            // Assert
            // Expected result : No Exception
            
        }
    }
}
