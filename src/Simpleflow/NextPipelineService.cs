// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// Represents a function to execute next middleware in the pipeline. 
    /// </summary>
    /// <param name="context"></param>
    public delegate void NextPipelineService<TArg>(FlowContext<TArg> context);
}
