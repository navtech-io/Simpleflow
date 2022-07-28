// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Services
{
    /// <summary>
    /// A service to run the machine code instructions
    /// </summary>
    public class ExecutionService : IFlowPipelineService
    {
        /// <inheritdoc />
        public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
        {
            // Add trace for debugging
            context.Trace?.CreateNewTracePoint(nameof(ExecutionService));

            var scriptHelperContext = new ScriptHelperContext(context.Output, 
                                                              context.Options?.CancellationToken ?? default);

            context.Internals.CompiledScript?.Invoke(context.Argument, context.Output, scriptHelperContext);

            next?.Invoke(context);
        }
    }
}
