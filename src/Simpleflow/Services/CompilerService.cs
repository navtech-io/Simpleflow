// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using Simpleflow.CodeGenerator;
using Simpleflow.Exceptions;

namespace Simpleflow.Services
{
    /// <summary>
    /// <see cref="CompilerService"/> to parse the instructions
    /// and generate machine code
    /// </summary>
    public class CompilerService : IFlowPipelineService
    {
        private readonly IFunctionRegister _functionRegister;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="functionRegister"></param>
        public CompilerService(IFunctionRegister functionRegister, IOptions options = null)
        {
            _functionRegister = functionRegister ?? throw new ArgumentNullException(nameof(functionRegister));
        }

        /// <inheritdoc />
        public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
        {

            // Add trace for debugging
            context.Trace?.CreateNewTracePoint(nameof(CompilerService));

            /* Compile if CompiledScript  is null,
               Not-null means, it might be supplied by cache service or 
               any other predecessor in pipeline.
               So here we don't need to run again  */

            if (context.Internals.CompiledScript == null)
            {
                var eventPublisher = new ParserEventPublisher();
                CheckFunctionExecutionPermissions(context, eventPublisher);

                context.Internals.CompiledScript =
                    SimpleflowCompiler.Compile<TArg>(context.Script,
                                                    new FunctionRegisterCoordinator(_functionRegister, context.FunctionRegister),
                                                    eventPublisher);

                context.Trace?.Write("Compiled");
            }
            else
            {
                context.Trace?.Write("Compilation Skipped");
            }

            next?.Invoke(context);
        }

        private static void CheckFunctionExecutionPermissions<TArg>(FlowContext<TArg> context, ParserEventPublisher eventPublisher)
        {
            eventPublisher.OnVisit = (type, data) =>
            {
                if (type == EventType.VisitFunctionOnAvail
                    && context.Options?.DenyFunctions != null)
                {
                    var functionName = data.ToString();
                    if (context.Options.DenyFunctions.Contains(functionName, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new AccessDeniedException($"Function '{functionName}' cannot be allowed to run in this context.");
                    }
                }

                if (type == EventType.VisitFunctionOnAvail
                    && context.Options?.AllowFunctions != null)
                {
                    var functionName = data.ToString();
                    if (!context.Options.AllowFunctions.Contains(functionName, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new AccessDeniedException($"Function '{functionName}' cannot be allowed to run in this context.");
                    }
                }
            };
        }


    }
}
