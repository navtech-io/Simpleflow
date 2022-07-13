// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Globalization;
using Xunit;

using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests.Compiler
{
    public class SimpleflowFullFeaturesTest
    {
        [Fact]
        public void AllScriptFeatures()
        {

            // Arrange
            var context = new SampleArgument { Id = 233 };
            var script =
            @"
                /* Declare variable */
                let a        = 2
                let b        = 5
                let text     = ""Hello, विश्वम्‌""
                let liberate = true 

                /* 5 x 233 -1.5 = 1163.5*/
                let value   = ( 2 + 3 ) * arg.Id - 1.5  

                let date    = $GetCurrentDate()
                let date1   = $GetCurrentDateTime ( timezone: ""Eastern Standard Time"" )


                /* Rules */
                rule when  a == 2 then
                    message ""Valid-1""
                
                rule when  ""x"" == text then
                    message ""Valid-xy""
                          
                rule when arg.Id == 233 and a == 2 then
                    message ""Valid-2""
                    message ""Valid-3""
                end rule
                
                /* Statements outside of the rules */
                message ""It works all the time""
                message date
                message date1

                /* Change variable */
                set a = 3  

                /* Output objects */
                output a
                output text
                output b
                output arg.Id
                output value
                
                /* Rules */
                rule when (arg.Id == 233 and a == 3) or 2 == 3 then
                      error ""Invalid-""
            ";

            // Act
            FlowOutput result= SimpleflowEngine.Run(script, context);


            // Assert
            Assert.Equal(actual: result.Messages.Count, expected: 6);
            Assert.Equal(actual: result.Messages[0], expected: "Valid-1");
            Assert.Equal(actual: result.Messages[1], expected: "Valid-2");
            Assert.Equal(actual: result.Messages[2], expected: "Valid-3");
            Assert.Equal(actual: result.Messages[3], expected: "It works all the time");
            Assert.Equal(actual: result.Messages[4], expected: DateTime.Now.Date.ToString(CultureInfo.CurrentCulture));

            Assert.Equal(actual: result.Errors.Count, expected: 1);
            Assert.Equal(actual: result.Errors[0], expected: "Invalid-");

            Assert.Equal(actual: result.Output.Count, expected:5);
            Assert.Equal(actual: result.Output["a"], expected: 3);
            Assert.Equal(actual: result.Output["b"], expected: 5);
            Assert.Equal(actual: result.Output["text"], expected: "Hello, विश्वम्‌");
            Assert.Equal(actual: result.Output["arg.Id"], expected:233);
            Assert.Equal(actual: result.Output["value"], expected: 1163.5M);

        }
    }
}
