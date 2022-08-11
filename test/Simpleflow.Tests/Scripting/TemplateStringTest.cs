// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Scripting
{
    public class TemplateStringTest
    {
        [Fact]
        public void TemplateString()
        {
            // Arrange
            var script =
                @"
                  let text = `abc {arg.id} {arg.value} xyz`
                  message text  
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument() { Id=1, Value = 2 });

            // Assert
            Assert.Single(output.Messages);
            Assert.Equal(actual: output.Messages[0], expected: "abc 1 2 xyz");
            
        }

        [Fact]
        public void MultilineTemplateString()
        {
            // Arrange
            var script =
                @"
                    let to   = ""John"" 
                    let from = ""Chris""

                    let v    = ` Hi {to},
                                 .....
                                 Thanks,
                                 {from}
                               `
                    message v
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new object() );

            // Assert
            Assert.Single(output.Messages);
            Assert.Equal(actual: output.Messages[0], 
                         expected:
                         
                         $" Hi John,{System.Environment.NewLine}                                 .....{System.Environment.NewLine}                                 Thanks,{System.Environment.NewLine}                                 Chris{System.Environment.NewLine}                               ");

        }
    }
}
