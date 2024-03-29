﻿// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

using Simpleflow.Tests.Helpers;
using Simpleflow.Exceptions;

namespace Simpleflow.Tests.Scripting
{
    public class JsonObjectTypeAndInvokeFunctionTest
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
        public void CheckEnum()
        {
            // Arrange  
            var script =
                @"
                    let w = { permission : ""Read"" }
                    
                    $MethodWithObjArg ( s: w )

                    output w

                ";

            var context = new SampleArgument();
            var register = new FunctionRegister().Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: Permission.Read , actual: (output.Output["w"] as MethodArgument).Permission);
        }


        [Fact]
        public void CheckIncorrectEnum()
        {
            // Arrange  
            var script =
                @"
                    let w = { permission : ""ReadXyz"" }
                    
                    $MethodWithObjArg ( s: w )
                ";

            var context = new SampleArgument();
            var register = new FunctionRegister().Add("MethodWithObjArg", (Func<MethodArgument, string>)MethodWithObjArg);

            // Act & Assert
            AssertEx.Throws<SimpleflowException>(String.Format(Resources.Message.RequestedEnumValueNotFound, "ReadXyz", "Permission"), 
                                                () => new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context));
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


        [Fact]
        public void CheckNestedObjectSyntax()
        {
            // Arrange  
            var script =
                @"
                    
                    let w = { 
                              Uid: ""S123"",  
                              child:{ 
                                      Id : 20 
                                    },
                              child2: {value: 10.5}  
                            }
                    
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



        [Fact]
        public void CheckArithEpxressionInFunctionParam()
        {
            // Arrange  
            var script =
                @"
                    
                    let w = { 
                              Uid: ""S123"",  
                              child:{ 
                                      Id : 20 
                                    },
                              child2: {value: 10.5}  
                            }
                    
                    let y = $MethodWithObjSuperArg ( s: w,  value: 5 + arg.value )
                    
                    output y
                ";

            var context = new SampleArgument() {  Value = 10.5M };
            var register = new FunctionRegister()
                .Add("MethodWithObjSuperArg", (Func<MethodSuperArgument, decimal, string>)MethodWithObjArg1);

            // Act
            FlowOutput output = new SimpleflowPipelineBuilder().AddCorePipelineServices(register).Build().Run(script, context);

            // Assert
            Assert.Equal(expected: "S123-20-0--False-NULL-0-15.5", actual: output.Output["y"].ToString());
        }

        private static string MethodWithObjArg1(MethodSuperArgument s, decimal value)
        {
            return s.ToString() + "-" + value;
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
