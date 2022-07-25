// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Simpleflow.Resources;
using Simpleflow.Services;

namespace Simpleflow
{
    /// <summary>
    /// Builds pipeline using middleware and with default services.
    /// </summary>
    public class SimpleflowPipelineBuilder : ISimpleflowPipelineBuilder
    {
        private readonly LinkedList<IFlowPipelineService> _pipelineServices;

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleflowPipelineBuilder"/>
        /// </summary>
        public SimpleflowPipelineBuilder()
        {
            _pipelineServices = new LinkedList<IFlowPipelineService>();
        }

        /// <inheritdoc />
        public IReadOnlyList<IFlowPipelineService> Services => _pipelineServices.ToList().AsReadOnly();

        /// <summary>
        /// Adds default core services
        /// <br /> <see cref="CacheService"/>
        /// <br /> <see cref="CompilerService"/>
        /// <br /> <see cref="ExecutionService"/> 
        /// </summary>
        /// <returns></returns>
        public ISimpleflowPipelineBuilder AddCorePipelineServices(IOptions options = null)
        {
            AddCoreServicesInternal(FunctionRegister.Default, options);

            return this;
        }

        /// <summary>
        /// Adds default core services
        /// <br /> <see cref="CacheService"/>
        /// <br /> <see cref="CompilerService"/>
        /// <br /> <see cref="ExecutionService"/> 
        /// </summary>
        /// <returns></returns>
        public ISimpleflowPipelineBuilder AddCorePipelineServices(IFunctionRegister activityRegister, IOptions options = null)
        {
            if (activityRegister == null)
            {
                throw  new ArgumentNullException(nameof(activityRegister));
            }

            AddCoreServicesInternal(activityRegister, options);

            return this;
        }

        /// <inheritdoc />
        public ISimpleflowPipelineBuilder AddPipelineServices(params IFlowPipelineService[] services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Validate: All services must be not null
            foreach (var flowPipelineService in services)
            {
                if (flowPipelineService == null)
                {
                    throw new ArgumentNullException(
                        paramName: nameof(services), 
                        message: Message.ServiceCannotBeNull);
                }

                _pipelineServices.AddLast(flowPipelineService);
            }

            return this;
        }

        /// <inheritdoc />
        public ISimpleflow Build()
        {
            return new Simpleflow(_pipelineServices);
        }

        private void AddCoreServicesInternal(IFunctionRegister activityRegister, IOptions options)
        {
            if (options != null && options.CacheOptions != null)
            {
                AddPipelineServices(new CacheService(options.CacheOptions));
            }
            else
            {
                AddPipelineServices(new CacheService());
            }

            AddPipelineServices(
                new CompilerService(activityRegister, options),
                new ExecutionService());
        }
    }
}
