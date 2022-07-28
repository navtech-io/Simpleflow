// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Threading;
using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class ContextCancellationTokenTest
    {
        [Fact]
        public void CancelCancellationToken()
        {
            // Arrange
            var script = "output context.cancellationToken.iscancellationrequested";

            CancellationTokenSource s = new CancellationTokenSource();
            s.Cancel(false);

            // Act & Assert
            Assert.Throws<OperationCanceledException>(() =>
            SimpleflowEngine.Run(script, new object(),
                    new FlowContextOptions
                    {
                        CancellationToken = s.Token
                    }));
        }

        [Fact]
        public void CancelCancellationTokenWhileRunningScript()
        {
            // Arrange
            var script = "output context.cancellationToken.iscancellationrequested";

            CancellationTokenSource s = new CancellationTokenSource();


            // Act
            var flow = new SimpleflowPipelineBuilder()
                    .AddPipelineServices(new Services.CacheService(),
                                         new Services.CompilerService(FunctionRegister.Default),
                                         new CancelAndExecutionTokenService(s))
                    .Build();

           var result = flow.Run(script, new object(),
                    new FlowContextOptions
                    {
                        CancellationToken = s.Token
                    });

            // Assert            
            Assert.Single(result.Output);
            Assert.True((bool)result.Output["context.CancellationToken.IsCancellationRequested"]);
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

        class CancelAndExecutionTokenService : IFlowPipelineService
        {
            CancellationTokenSource _source;
            public CancelAndExecutionTokenService(CancellationTokenSource source)
            {
                _source = source;
            }
            public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
            {
                _source.Cancel();
                var es = new Services.ExecutionService();
                es.Run(context, next);
            }
        }
    }
}
