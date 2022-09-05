// Copyright (c) navtech.io
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public static class SimpleflowEngine
    {
        static readonly ISimpleflow Simpleflow;

        static SimpleflowEngine()
        {
            var engine
                = new SimpleflowPipelineBuilder().AddCorePipelineServices();

            Simpleflow = engine.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="script"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static FlowOutput Run<TInput>(string script, TInput context)
        {
            return Simpleflow.Run(script, context);
        }

        public static FlowOutput Run<TInput>(string script, TInput context, IContextOptions options)
        {
            return Simpleflow.Run(script, context, options);
        }

        public static FlowOutput Run<TArg>(string script, TArg argument, IFunctionRegister register)
        {
            return Simpleflow.Run(script, argument, register);
        }

        public static FlowOutput Run<TArg>(string script, TArg argument, IContextOptions options, IFunctionRegister register)
        {
            return Simpleflow.Run(script, argument, options, register);
        }
    }
}
