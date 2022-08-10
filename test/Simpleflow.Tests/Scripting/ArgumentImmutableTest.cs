// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using Xunit;
using Simpleflow.Exceptions;
using Simpleflow.Tests.Helpers;
using System;

namespace Simpleflow.Tests.Scripting
{
    public class ArgumentImmutableTest
    {
        [Fact]
        public void ArgumentImmutable()
        {
            // Arrange
            var arg = new SampleArgument { Id = 10 };
            var script =
                @"
                    partial set arg = { id:  20 } 
                ";

            // Act & Assert

            Assert.Throws<ArgumentImmutableExeception>(
                    () => SimpleflowEngine.Run(script,
                                               arg,
                                               new FlowContextOptions { AllowArgumentToMutate = false }));
        }
        [Fact]
        public void ArgumentImmutableInlineErrorHandler()
        {
            // Arrange
            var arg = new SampleArgument { Id = 10 };
            var script =
                @"
                    partial set arg, err = { id:  20 } 
                    output err
                ";

            // Act & Assert
            var result = SimpleflowEngine.Run(script,
                                        arg,
                                        new FlowContextOptions { AllowArgumentToMutate = false });
            
            // Assert
            Assert.IsType<ArgumentImmutableExeception>(result.Output["err"]);
        }


        [Fact]
        public void ArgumentImmutableInlineErrorHandlerWithChildObj()
        {
            // Arrange
            var arg = new MethodSuperArgument { Child = new MethodArgument() };
            var script =
                @"
                    let c = arg.child
                    partial set c, err = { id:  20 } 

                    output err
                ";

            // Act
            var result = SimpleflowEngine.Run(script,
                                        arg,
                                        new FlowContextOptions
                                        {
                                            AllowArgumentToMutate = false
                                        });
            // Assert
            Assert.IsType<ArgumentImmutableExeception>(result.Output["err"]);
        }

        [Fact]
        public void AllowOtherObjectsMutable()
        {
            // Arrange  
            var script =
                @"
                    let c = { Id : 20 }
                    let w = { Uid: ""S123"", child: c }
                    
                    let y = $MethodWithObjSuperArg ( s: w )
                    
                    partial set w = {
                        Uid: ""123""
                    }

                    output w
                ";

            var context = new SampleArgument();
            var register = new FunctionRegister().Add("MethodWithObjSuperArg", (Func<MethodSuperArgument, string>)MethodWithObjSuperArg);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder()
                                        .AddCorePipelineServices(register)
                                        .Build()
                                        .Run(script, context, new FlowContextOptions
                                        {
                                            AllowArgumentToMutate = false
                                        });

            // Assert
            Assert.Equal(expected: "123-20-0--False-NULL-0", actual: output.Output["w"].ToString());
        }

        private static string MethodWithObjSuperArg(MethodSuperArgument s)
        {
            return s.ToString();
        }
    }
}
