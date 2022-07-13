// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Simpleflow.CodeGenerator;

namespace Simpleflow.Services
{
    /// <summary>
    /// <see cref="CompilerService"/> to parse the instructions
    /// and generate machine code
    /// </summary>
    public class CompilerService : IFlowPipelineService
    {
        private readonly IFunctionRegister _activityRegister;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="activityRegister"></param>
        public CompilerService(IFunctionRegister activityRegister, IOptions options = null)
        {
            _activityRegister = activityRegister ?? throw new ArgumentNullException(nameof(activityRegister));
        }

        /// <inheritdoc />
        public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
        {
            /* Compile if CompiledScript  is null,
               Not-null means, it might be supplied by cache service or 
               any other predecessor in pipeline.
               So here we don't need to run again  */

            if (context.Internals.CompiledScript == null)
            {
                context.Internals.CompiledScript = SimpleflowCompiler.Compile<TArg>(context.Script, _activityRegister);

                context.Trace.Write("Compiled");
            }
            else
            {
                context.Trace.Write("Compilation Skipped");
            }
            
            next?.Invoke(context);
        }
    }
}
