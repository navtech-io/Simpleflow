// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Simpleflow.Exceptions;

namespace Simpleflow.Services
{
    /// <summary>
    /// A service to run the machine code instructions
    /// </summary>
    public class ExecutionService : IFlowPipelineService
    {
        readonly IOptions _options;

        /// <summary>
        /// 
        /// </summary>
        public ExecutionService()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ExecutionService(IOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
        {
            // Add trace for debugging
            context.Trace?.CreateNewTracePoint(nameof(ExecutionService));

            var scriptContext = new RuntimeContext(context.Output,
                                                             context.Options?.CancellationToken ?? default);

            try
            {
                context.Internals.CompiledScript?.Invoke(context.Argument, context.Output, scriptContext);
            }
            catch(Exception ex)
            {
                // Get statement where error has occurred
                var lines = context.Script?.Split('\n') ?? new string[] { };
                var code = lines.Length >= scriptContext.LineNumber && scriptContext.LineNumber > 0 ? lines[scriptContext.LineNumber-1] : context.Script;

                // throw
                throw new SimpleflowRuntimeException(ex.Message, scriptContext.LineNumber, code, ex);
            }

            next?.Invoke(context);
        }
    }
}
