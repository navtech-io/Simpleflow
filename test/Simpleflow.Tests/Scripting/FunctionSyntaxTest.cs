// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;
using Simpleflow.Tests.Helpers;
using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class FunctionSyntaxTest
    {
        [Fact]
        public void FunctionWithExpressions()
        {
            // Arrange  
            var script =
                @"
                    let v = 23
                    let y = $MethodWithObjArg ( s: {
                                        Id : arg.Id, 
                                        Value : 3.4, 
                                        Text: ""Abc"",  
                                        NullCheck: none, 
                                        IsValid: true, 
                                        CheckIdentifer: v })
                    output y
                ";

            var context = new SampleArgument() { Id = 25 };
            var register = new FunctionRegister().Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "25-3.4-Abc-True-NULL-23", actual: output.Output["y"]);

        }

        private static string MethodWithObjArg(MethodArgument s)
        {
            return s.ToString();
        }

    }
}
