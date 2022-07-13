// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Collections.Generic;

namespace Simpleflow
{
    /// <summary>
    /// Represents simple flow builder abstraction.
    /// Builds pipeline using middleware and with default services.
    /// </summary>
    public interface ISimpleflowPipelineBuilder
    {
        /// <summary>
        /// Gets list of services that have been added
        /// </summary>
        IReadOnlyList<IFlowPipelineService> Services { get; }

        /// <summary>
        /// Adds default core services. Please check the implementation
        /// class documentation for added default services.
        /// </summary>
        /// <returns>The <see cref="ISimpleflowPipelineBuilder"/></returns>
        ISimpleflowPipelineBuilder AddCorePipelineServices(IOptions options = null);


        /// <summary>
        /// Adds default core services. Please check the implementation
        /// class documentation for added default services.
        /// </summary>
        /// <returns>The <see cref="ISimpleflowPipelineBuilder"/></returns>
        ISimpleflowPipelineBuilder AddCorePipelineServices(IFunctionRegister functionsConfig, IOptions options = null);

        /// <summary>
        /// Adds middleware services to simpleflow 
        /// </summary>
        /// <param name="services">Specify the list of services to be added</param>
        /// <returns>The <see cref="ISimpleflowPipelineBuilder"/> </returns>
        ISimpleflowPipelineBuilder AddPipelineServices(params IFlowPipelineService[] services);

        /// <summary>
        /// Builds pipeline and provides core engine
        /// with all the middleware that have been configured
        /// </summary>
        /// <returns>The <see cref="ISimpleflow"/></returns>
        ISimpleflow Build();
    }
}
