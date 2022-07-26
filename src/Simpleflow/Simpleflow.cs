// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Collections.Generic;

namespace Simpleflow
{
    /// <summary>
    /// Simpleflow executes instructions. It's thread safe.
    /// </summary>
    public sealed class Simpleflow : ISimpleflow
    {
        private readonly LinkedList<IFlowPipelineService> _services;

        internal Simpleflow(LinkedList<IFlowPipelineService> services)
        {
            _services = services;
        }

        /// <inheritdoc />
        public FlowOutput Run<TArg>(string script, TArg argument)
        {
            ArgumentException.ThrowIfNullOrEmpty(script);
            ArgumentException.ThrowIfNull(argument);

            return RunInternal(script, argument, options: null, config: null);
        }

        /// <inheritdoc />
        public FlowOutput Run<TArg>(string script, TArg argument, IContextOptions options)
        {
            ArgumentException.ThrowIfNullOrEmpty(script);
            ArgumentException.ThrowIfNull(argument);
            ArgumentException.ThrowIfNull(options);

            return RunInternal(script, argument, options, config: null);
        }

        /// <inheritdoc />
        public FlowOutput Run<TArg>(string script, TArg argument, IFunctionRegister config)
        {
            ArgumentException.ThrowIfNullOrEmpty(script);
            ArgumentException.ThrowIfNull(argument);
            ArgumentException.ThrowIfNull(config);

            return RunInternal(script, argument, options: null, config);
        }

        /// <inheritdoc />
        public FlowOutput Run<TArg>(string script, TArg argument, IContextOptions options, IFunctionRegister config)
        {

            ArgumentException.ThrowIfNullOrEmpty(script);
            ArgumentException.ThrowIfNull(argument);
            ArgumentException.ThrowIfNull(options);
            ArgumentException.ThrowIfNull(config);

            return RunInternal(script, argument, options, config);
        }

        private FlowOutput RunInternal<TArg>(string script, TArg argument, IContextOptions options, IFunctionRegister config)
        {
            var context = new FlowContext<TArg>()
            {
                Script = script,
                Argument = argument,
                Options = options,
                FunctionRegister = config
            };

            RunPipelineService<TArg>(_services.First, context);
            return context.Output;
        }


        private void RunPipelineService<TArg>(LinkedListNode<IFlowPipelineService> serviceNode, 
                                              FlowContext<TArg> input)
        {
            NextPipelineService<TArg> next =
                serviceNode.Next != null ?
                    (flowInput) => RunPipelineService<TArg>(serviceNode.Next, flowInput)
                    : default(NextPipelineService<TArg>);

            serviceNode.Value.Run(input, next);
        }
    }
}
