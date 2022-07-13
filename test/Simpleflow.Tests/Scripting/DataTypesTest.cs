// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

using Simpleflow.CodeGenerator;
using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests.Scripting
{
    public class DataTypesTest
    {
        [Fact]
        public void CheckAllDataTypes()
        {
            //TODO
            /* Number (positive/negative integer/float/decimal) default 0
               String (default "")
               Bool   (default false)
               Array of integers, strings, booleans
               (default type... (variable in script like context))
               (type.number.empty/type.string.empty/type.bool.empty/type.array.integer.empty
               /type.array.string.empty)

               Object ( Determine its type ) = .. Determine automatically when its used in functions
               // Object can be used only with functions, to determine its type  -- 
               // otherwise object will be declared as dictionary to use in script
               // due to script security, it will not allow to define type for object 

               Access array a`1.id  a`2.id  
               -- Maybe this way, create type without specify explicitly let o = type.object for func-param getdata-name, xyz

               Auto typecasting when passing variable to function

              TEST: Support without space 10+10 -- or throw proper exception
            */
            //
            // Arrange
            var script =
                @"
                    let int = 10
                    let decimal = 10.5
                    let text = ""TEXT""
                    let boolean = true

                    output int
                    output decimal
                    output text
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.IsType<int>(output.Output["int"]);
            Assert.IsType<decimal>(output.Output["decimal"]);
            Assert.IsType<string>(output.Output["text"]);

        }

    }
}
