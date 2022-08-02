// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class CommentTest
    {
        [Fact]
        public void Comments()
        {
            // Arrange
            var script =
                @"
                    # test single line

                    message  ""abc"" # test single line here

                    /* multi 
                    line   */
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new object());

            // Assert
            Assert.Single(output.Messages);
        }
    }
}
