// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

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
            var register = new FunctionRegister().Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);
            
            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "25-3.4-Abc-True-NULL-23", actual: output.Output["x"]);
            Assert.Equal(expected: "25-3.4-Abc-True-NULL-23", actual: output.Output["y"]);

        }


        [Fact]
        public void CheckReplaceObject()
        {
            // Arrange  
            var script =
                @"
                    let v = 23
                    let w = {Id : arg.Id, Value : 3.4, Text: ""Abc"",  NullCheck: none, IsValid: true, CheckIdentifer: v }
                    
                    let y = $MethodWithObjArg ( s: w )

                    /*full replace*/
                    set w = { Id: 100 } 
                    output w

                ";

            var context = new SampleArgument() { Id = 25 };
            var register = new FunctionRegister().Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "100-0--False-NULL-0", actual: output.Output["w"].ToString());

        }


        [Fact]
        public void CheckEmptyObject()
        {
            // Arrange  
            var script =
                @"
                    let w = { }
                    
                    let y = $MethodWithObjArg ( s: w )

                    output w

                ";

            var context = new SampleArgument();
            var register = new FunctionRegister().Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "0-0--False-NULL-0", actual: output.Output["w"].ToString());
        }


        [Fact]
        public void CheckNestedObject()
        {
            // Arrange  
            var script =
                @"
                    let c = { Id : 20 }
                    let w = { Uid: ""S123"", child: c }
                    
                    let y = $MethodWithObjSuperArg ( s: w )
                    
                    output w
                ";

            var context = new SampleArgument();
            var register = new FunctionRegister().Add("MethodWithObjSuperArg", (Func<MethodSuperArgument, string>)MethodWithObjSuperArg);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "S123-20-0--False-NULL-0", actual: output.Output["w"].ToString());
        }


        private static string MethodWithObjArg(MethodArgument s)
        {
            return s.ToString();
        }

        private static string MethodWithObjSuperArg(MethodSuperArgument s)
        {
            return s.ToString();
        }
    }
}
