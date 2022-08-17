// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Scripting
{
    public class StringTest
    {
       
        [Fact]
        public void StringLiteralTest()
        {
            // Arrange
            var script =
                @"
                  let text  = 'test\''
                  let text2 = ""test\""""
                  message text 
                  message text2
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument() { Id = 1, Value = 2 });

            // Assert
            
            Assert.Equal(actual: output.Messages[0], expected: "test'");
            Assert.Equal(actual: output.Messages[1], expected: "test\"");

        }

    }
}
