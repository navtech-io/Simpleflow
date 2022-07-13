// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class ScriptContextArgTest
    {
        [Fact]
        public void ContextHasErrors()
        {
            // Arrange
            var script =
                @$"
                    error ""test""
                    output context.haserrors
                ";

            // Act & Assert
            var output = SimpleflowEngine.Run(script, new object());
            Assert.Equal(true, output.Output["context.haserrors"]);
        }

        [Fact]
        public void ContextHasNoErrors()
        {
            // Arrange
            var script =
                @$"
                    output context.HasErrors
                ";

            // Act & Assert
            var output = SimpleflowEngine.Run(script, new object());
            Assert.Equal(false, output.Output["context.HasErrors"]);
        }
    }
}
