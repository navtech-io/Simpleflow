// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

using Simpleflow.CodeGenerator;
using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests.Scripting
{
    public class JsonObjectTypeTest
    {

        [Fact]
        public void CheckObject()
        {
            // Arrange  
            var script =
                @"
                    let v = 23
                    let w = {Id : arg.Id, Value : 3.4, Text: ""Abc"",  NullCheck: none, IsValid: true, CheckIdentifer: v }
                    let x = none

                    let y = $MethodWithObjArg ( s: w )
                    set x = $MethodWithObjArg ( s: w )

                    output x
                    output y
                ";

            var context = new SampleArgument() { Id = 25 };
            var register = FunctionRegister.Default.Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);
            
            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "25-3.4-Abc-True-NULL-23", actual: output.Output["x"]);
            Assert.Equal(expected: "25-3.4-Abc-True-NULL-23", actual: output.Output["y"]);

        }

        private static string MethodWithObjArg(MethodArgument s)
        {
            return s.ToString();
        }

        class MethodArgument
        {
            public int Id { get; set; }
            public decimal Value { get; set; }
            public string Text { get; set; }
            public bool IsValid { get; set; }
            public string NullCheck { get; set; }
            public int CheckIdentifer { get; set; }
            public override string ToString()
            {
                return $"{Id}-{Value}-{Text}-{IsValid}-{NullCheck??"NULL"}-{CheckIdentifer}";
            }
        }

    }

   
}
