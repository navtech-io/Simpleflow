// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class AccessPropertyOrFieldTest
    {

        [Fact]
        public void DynamicArgument()
        {
          
            // Arrange
            var script =
                @"
                    output arg.name
                    output arg.data
                ";

            // Act 
            var output = SimpleflowEngine.Run(script, new FieldsArg { Name="Field", Data="Prop" });

            // Assert
            Assert.Equal("Field", output.Output["arg.name"]);
            Assert.Equal("Prop", output.Output["arg.data"]);
        }

        public class FieldsArg
        {
            public string Name;
            public string Data { get; set; }

        }
    }
}
