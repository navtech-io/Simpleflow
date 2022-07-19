// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.IO;
using Simpleflow.Services;

using Xunit;
using Xunit.Abstractions;

namespace Simpleflow.Tests
{
    public class SimpleflowTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ISimpleflow _flow;

        public SimpleflowTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var engine
                = new SimpleflowPipelineBuilder()
                    .AddCorePipelineServices()
                    .AddPipelineServices(new SimpleflowTest.LoggingService());

            _flow = engine.Build();
        }

        [Theory]
        [InlineData(1, "First")]
        [InlineData(2, "Second")]
        public void BuildAndRunSimpleflow_CheckWhetherEachServiceIsInvoked(int id, string expect)
        {
            // Arrange
            string script = @$"rule when arg.Id > 0 then 
                                    message ""Member exists""
                               /*end rule*/
                               rule when arg.Id == {id} then 
                                    message ""The {expect} Member""
                               
                          ";

            // Act
            FlowOutput result = _flow.Run(script, new Member { Id = id});


            // Assert
            SimpleflowTrace trace = (SimpleflowTrace)result.Output["Trace"];
            StringReader reader = new StringReader(trace.ToString());

            Assert.Equal(actual: reader.ReadLine(),
                         expected: $"Simpleflow.Services.{nameof(CacheService)}");

            Assert.Equal(actual: reader.ReadLine(),
                         expected: $"Simpleflow.Services.{nameof(CompilerService)}");

            Assert.Equal(actual: reader.ReadLine(),
                         expected: $"Simpleflow.Services.{nameof(ExecutionService)}");

            Assert.Equal(actual: reader.ReadLine(),
                         expected: $"Simpleflow.Tests.{nameof(SimpleflowTest)}+{nameof(LoggingService)}");

            Assert.Equal(actual: result.Output["Log-Output"],
                         expected: script);

            Assert.Equal(actual: result.Messages[0],
                expected: "Member exists");

            Assert.Equal(actual: result.Messages[1],
                expected: $"The {expect} Member");

            _testOutputHelper.WriteLine(trace.GetLogs());

        }

        [Fact]
        public void Run_CheckArgumentException()
        {
            // Arrange
            var script = @"rule when arg.Test = 1 then 
                                message 'test'";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _flow.Run(script, new { Test = 1 }, config: null));
            Assert.Throws<ArgumentNullException>(() => _flow.Run(script, new { Test = 1, Abc=2 }, options: null));
            Assert.Throws<ArgumentNullException>(() => _flow.Run(script, new { Test = 1 }, options: null, config: null));
            Assert.Throws<ArgumentNullException>(() => _flow.Run(script, new { Test = 1 }, options: new FlowContextOptions(), config: null));
            Assert.Throws<ArgumentNullException>(() => _flow.Run(script, new { Test = 1 }, options: null, config: new FunctionRegister()));
        }

        #region  Test Support Classes

        class LoggingService : IFlowPipelineService
        {
            public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
            {
                context.Output.Output.Add("Log-Output", context.Script);
                context.Output.Output.Add("Trace", context.Trace);
               

                next?.Invoke(context);
            }
        }

        class Member
        {
            public int Id { get; set; }
        }

        #endregion

    }


}
