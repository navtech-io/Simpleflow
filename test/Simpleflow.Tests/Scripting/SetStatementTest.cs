// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.CodeGenerator;
using Simpleflow.Tests.Helpers;
using Simpleflow.Exceptions;

namespace Simpleflow.Tests.Scripting
{
    public class SetStatementTest
    {
        [Fact]
        public void MutateStatement()
        {
            // TODO support property value change : mutate  arg.Id = 30

            // Arrange
            var context = new SampleArgument { Id = 10 };
            var script =
                @$"
                  let value = 5
                  
                  message value

                  set value = 20 + 5
                    
                  message value
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.Equal(actual: output.Messages.Count, expected: 2);

            Assert.Equal(actual: output.Messages[0], expected: "5");
            Assert.Equal(actual: output.Messages[1], expected: "25");

        }

        [Fact]
        public void TryToChangeVariableWithoutMutateKeyword()
        {
            // Arrange
            var context = new SampleArgument { Id = 10 };
            var script =
                @$"
                  let value = 5
                  
                  value = 20
                ";

            // Act & Assert
            Assert.Throws<SyntaxErrorException>(() => SimpleflowEngine.Run(script, new SampleArgument()));
            
        }
    }
}
