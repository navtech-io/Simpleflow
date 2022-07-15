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
    }

   

}
