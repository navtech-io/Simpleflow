// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

namespace Simpleflow.Tests.Infrastructure
{
    public  class FlowContextOptionsTest
    {
        [Fact]
        public void CheckFlowContextOptionCacheId()
        {
            // Arrange
            string script = @"
                              message ""test""
                            ";
            string id = System.Guid.NewGuid().ToString();
            var options = new FlowContextOptions { Id = id };

            // Act
            FlowOutput result = new SimpleflowPipelineBuilder()
                                    .AddCorePipelineServices(FunctionRegister.Default)
                                    .AddPipelineServices(new LoggingService())
                                    .Build()
                                    .Run(script, 
                                         new object(),
                                         options);

            // Assert
            SimpleflowTrace trace = (SimpleflowTrace)result.Output["Trace"];
            var logs = trace.GetLogs();

            Assert.Contains(expectedSubstring: id , actualString: logs);
        }

        [Fact]
        public void CheckFlowContextOptionWithDenyFunction()
        {
            // Arrange
            string script = @"
                              message ""test""
                            ";
            string id = System.Guid.NewGuid().ToString();
            var options = new FlowContextOptions 
                                    { 
                                      Id = id, 
                                      DenyOnlyFunctions=  new string[] { "GetCurrentDateTime" }
                                    };

            // Act
            FlowOutput result = new SimpleflowPipelineBuilder()
                                    .AddCorePipelineServices(FunctionRegister.Default)
                                    .AddPipelineServices(new LoggingService())
                                    .Build()
                                    .Run(script,
                                         new object(),
                                         options);

            // Assert
            SimpleflowTrace trace = (SimpleflowTrace)result.Output["Trace"];
            var logs = trace.GetLogs();

            Assert.Contains(expectedSubstring: id, actualString: logs);
        }

        class LoggingService : IFlowPipelineService
        {
            public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
            {
                context.Output.Output.Add("Trace", context.Trace);
                next?.Invoke(context);
            }
        }
    }
}
