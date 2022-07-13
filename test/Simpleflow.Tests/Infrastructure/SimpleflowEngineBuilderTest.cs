// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Diagnostics;
using Simpleflow.Services;
using Xunit;

namespace Simpleflow.Tests
{
    public class SimpleflowEngineBuilderTest
    {
        [Fact]
        public void AddPipelineServicesWithNullValues_ThrowsArgumentNullException()
        {
            // Arrange 
            IFlowPipelineService[] services = new IFlowPipelineService[2] ;
            var engine = new SimpleflowPipelineBuilder();
                
            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => engine.AddPipelineServices(null));
            Assert.Throws<ArgumentNullException>(() => engine.AddPipelineServices(services));

        }

        [Fact]
        public void AddPipelineServices_ContainerHasAllRegisteredServices()
        {
            // Arrange & Act
            var engine = new SimpleflowPipelineBuilder()
                                                .AddCorePipelineServices()
                                                .AddPipelineServices(new LoggingService());

            // Assert
            Assert.IsType<CacheService>(engine.Services[0]);
            Assert.IsType<CompilerService>(engine.Services[1]);
            Assert.IsType<ExecutionService>(engine.Services[2]);
            Assert.IsType<LoggingService>(engine.Services[3]);
        }

        public class LoggingService : IFlowPipelineService
        {
            public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
            {
                next?.Invoke(context);
            }
        }
    }

    
}
