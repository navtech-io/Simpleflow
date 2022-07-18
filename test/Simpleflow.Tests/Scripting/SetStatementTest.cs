// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

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
                @"
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
                @"
                  let value = 5
                  
                  value = 20
                ";

            // Act & Assert
            Assert.Throws<SyntaxException>(() => SimpleflowEngine.Run(script, new SampleArgument()));
            
        }

        [Fact]
        public void CheckUndeclaredVariableExceptionForSet()
        {
            // Arrange
            var context = new SampleArgument { Id = 10 };
            var script =
                @"
                  set x = 20
                ";

            // Act & Assert
            AssertEx.Throws<UndeclaredVariableException>(
                    ex => Assert.Equal("x", ex.VariableName), 
                    () => SimpleflowEngine.Run(script, new SampleArgument()));
        }

        [Fact]
        public void PartialSet()
        {
            // Arrange
            var arg = new SampleArgument { Id = 10 };
            var script =
                @"
                    partial set arg = { id: 34, value: 10 }

                ";

            // Act & Assert
            var result = SimpleflowEngine.Run(script, arg);

            Assert.Equal(expected: 34, arg.Id);
            Assert.Equal(expected: 10, arg.Value);
        }

        [Fact]
        public void CheckObjectWithPartialSet()
        {
            // Arrange  
            var script =
                @"
                    let w = {Id : arg.Id, Value : 3.4, Text: ""Abc"",  NullCheck: none, IsValid: true, CheckIdentifer: 23 }
                    
                    $MethodWithObjArg ( s: w )

                    partial set w = { text : ""xyz""}

                    output w
                ";

            var context = new SampleArgument() { Id = 25 };
            var register = new FunctionRegister().Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "25-3.4-xyz-True-NULL-23", actual: output.Output["w"].ToString());

        }

        private static string MethodWithObjArg(MethodArgument s)
        {
            return s.ToString();
        }
    }
}
