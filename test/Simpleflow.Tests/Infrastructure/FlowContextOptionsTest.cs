// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Simpleflow.Exceptions;
using Xunit;

namespace Simpleflow.Tests.Infrastructure
{
    public class FlowContextOptionsTest
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

            Assert.Contains(expectedSubstring: id, actualString: logs);
        }

        [Fact]
        public void CheckFlowContextOptionWithDenyFunction()
        {
            // Arrange
            string script = @"
                              let d = $GetCurrentDateTime()
                              message d
                            ";
            string id = System.Guid.NewGuid().ToString();
            var options = new FlowContextOptions
            {
                Id = id,
                DenyFunctions = new string[] { "GetCurrentDateTime" }
            };

            // Act & Assert
            Assert.Throws<AccessDeniedException>(
                () => new SimpleflowPipelineBuilder()
                            .AddCorePipelineServices(FunctionRegister.Default)
                            .AddPipelineServices(new LoggingService())
                            .Build()
                            .Run(script,new object(), options));
        }

        [Fact]
        public void CheckFlowContextOptionWithAllowOnlyFunction()
        {
            // Arrange
            string script = @"
                              let d = $GetCurrentDate()
                              message d
                            ";
            string id = System.Guid.NewGuid().ToString();
            var options = new FlowContextOptions
            {
                Id = id,
                AllowFunctions = new string[] { "GetCurrentDateTime" }
            };

            // Act & Assert
            Assert.Throws<AccessDeniedException>(
                () => new SimpleflowPipelineBuilder()
                            .AddCorePipelineServices(FunctionRegister.Default)
                            .AddPipelineServices(new LoggingService())
                            .Build()
                            .Run(script, new object(), options));
        }

        [Fact]
        public void CheckFlowContextOptionWithAllowOnlyFunctionWithoutError()
        {
            // Arrange
            string script = @"
                              let d = $GetCurrentDateTime()
                              message d
                            ";
            string id = System.Guid.NewGuid().ToString();
            var options = new FlowContextOptions
            {
                Id = id,
                AllowFunctions = new string[] { "GetCurrentDateTime" }
            };

            // Act 
            var result =
                    new SimpleflowPipelineBuilder()
                            .AddCorePipelineServices(FunctionRegister.Default)
                            .AddPipelineServices(new LoggingService())
                            .Build()
                            .Run(script, new object(), options);
            // Assert
            Assert.Single(result.Messages);
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
