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
            // TODO
            // Does it have permission to mutate vars ?
            // Send these privileges as an argument
            // { ReadOnly = true, Context = '' ,  permissions = new [] {}  }
            // if ActivityAllowed('sendmail', permissions) { sendmail () }

            
            var flowInput = new FlowInput<TArg>( context.Argument, options:null );
            var scriptHelperContext = new ScriptHelperContext(context.Output);

            context.Internals.CompiledScript?.Invoke(flowInput, context.Output, scriptHelperContext);

            next?.Invoke(context);
        }
    }
}
