// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Threading;
using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class ContextCancellationTokenTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CancelCancellationToken(bool cancel)
        {
            // Arrange
            var script = "output context.cancellationToken.iscancellationrequested";

            CancellationTokenSource s = new CancellationTokenSource();
            if (cancel)
            {
                s.Cancel(false);
            }

            // Act
            var output = SimpleflowEngine.Run(script, new object(), 
                    new FlowContextOptions { 
                            CancellationToken = s.Token
                    });

            // Assert            
            Assert.Single(output.Output);
            Assert.Equal(cancel, (bool)output.Output["context.CancellationToken.IsCancellationRequested"]);
        }

        [Fact]
        public void CancellationTokenWithoutPassingToken()
        {
            // Arrange
            var script = "output context.cancellationToken.iscancellationrequested";

            // Act
            var output = SimpleflowEngine.Run(script, new object());

            // Assert            
            Assert.Single(output.Output);
        }
    }
}
