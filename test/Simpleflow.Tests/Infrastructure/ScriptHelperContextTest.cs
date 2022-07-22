﻿
using Xunit;

namespace Simpleflow.Tests.Infrastructure
{
    public class ScriptHelperContextTest
    {
        [Fact]
        public void HasErrorsMessagesOutput()
        {
            // Arrange
            
            var script =
                @"
                    let x = 10    
                    error ""test""
                    message ""abc""

                    output x
                ";
            
            // Act 
            var result = SimpleflowEngine.Run(script, new object());
            var scriptHelperContext = new ScriptHelperContext(result);

            // Assert
            Assert.True(scriptHelperContext.HasErrors);
            Assert.True(scriptHelperContext.HasMessages);
            Assert.True(scriptHelperContext.HasOutput);

        }
        [Fact]
        public void HasNoErrorsMessagesOutput()
        {
            // Arrange

            var script =
                @"
                    let x = 10
                ";

            // Act 
            var result = SimpleflowEngine.Run(script, new object());
            var scriptHelperContext = new ScriptHelperContext(result);

            // Assert
            Assert.False(scriptHelperContext.HasErrors);
            Assert.False(scriptHelperContext.HasMessages);
            Assert.False(scriptHelperContext.HasOutput);

        }
    }
}