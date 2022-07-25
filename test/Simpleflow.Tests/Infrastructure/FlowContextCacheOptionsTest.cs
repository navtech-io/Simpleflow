// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Simpleflow.Exceptions;
using Xunit;

namespace Simpleflow.Tests.Infrastructure
{
    public class FlowContextCacheOptionsTest
    {
        [Fact]
        public void CheckFlowContextCacheOptions()
        {
            // Arrange
            string script = @"
                              message ""test""
                            ";
            string id = System.Guid.NewGuid().ToString();

            var options = new FlowContextOptions { 
                    CacheOptions = new CacheOptions { 
                        AbsoluteExpiration = System.DateTimeOffset.Now.AddHours(1),
                        SlidingExpiration = System.TimeSpan.FromMinutes(3),
                        HashingAlgToIdentifyScriptUniquely = "SHA256"
                    }
            };

            // Act
            
            FlowOutput result = new SimpleflowPipelineBuilder()
                                    .AddPipelineServices(new CacheService2())
                                    .AddPipelineServices(new Services.CompilerService(FunctionRegister.Default))
                                    .AddPipelineServices(new Services.ExecutionService())
                                    .Build()
                                    .Run(script,
                                         new object(),
                                         options);

            // Assert
        }

        class CacheService2 : Services.CacheService
        {
            protected override string GetScriptUniqueId(CacheOptions contextCacheOptions, string script)
            {
                Assert.NotNull(contextCacheOptions);
                Assert.NotNull(contextCacheOptions.AbsoluteExpiration);

                return base.GetScriptUniqueId(contextCacheOptions, script);
            }
        }
    }
}
