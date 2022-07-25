// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Simpleflow.Exceptions;
using Xunit;

namespace Simpleflow.Tests.Infrastructure
{
    public class FlowContextFunctionRegisterOptionsTest
    {
        [Fact]
        public void CheckFlowContextFunctionRegisterOptions()
        {
            // Arrange
            string script = @"
                              let d         = $getdate()
                              let override  = $getcurrentdate()

                              message d
                              message override

                            ";
            string id = System.Guid.NewGuid().ToString();
            var options = new FlowContextOptions { Id = id,
            };
            var funcRegister = new FunctionRegister();
            funcRegister.Add("getdate", (Func<string>)GetDate);
            funcRegister.Add("getcurrentdate", (Func<string>)GetDate);

            // Act
            FlowOutput result = new SimpleflowPipelineBuilder()
                                    .AddCorePipelineServices(FunctionRegister.Default)
                                    .Build()
                                    .Run(script,
                                         new object(),
                                         options,
                                         funcRegister);

            // Assert
            Assert.Equal(2, result.Messages.Count);
            Assert.Equal("test", result.Messages[0]);
            Assert.Equal("test", result.Messages[1]);

        }


        static string GetDate()
        {
            return "test";
        }

    }
}
