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
            AddPipelineServices(
                new CacheService(),
                new CompilerService(FunctionRegister.Default, options),
                new ExecutionService()); 

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

            AddPipelineServices(
                new CacheService(),
                new CompilerService(activityRegister, options),
                new ExecutionService());

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
    }
}
