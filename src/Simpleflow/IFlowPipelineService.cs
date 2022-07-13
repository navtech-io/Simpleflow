// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// Represents middleware abstraction for pipeline configuration.
    /// </summary>
    public interface IFlowPipelineService
    {
        /// <summary>
        /// Run pipeline service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"><paramref name="next"/> would be null if no service in queue after the current one</param>
        /// <returns></returns>
        void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next);
    }
}
