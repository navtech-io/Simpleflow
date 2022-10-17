// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Threading;

namespace Simpleflow
{
    /// <summary>
    /// This class is for internal purpose only
    /// </summary>
    public sealed class RuntimeContext
    {
        readonly FlowOutput _flowOutput;
        readonly CancellationToken _token;

        internal RuntimeContext(FlowOutput flowOutput,  CancellationToken token)
        {
            _flowOutput = flowOutput;
            _token = token;
        }
        
        /// <summary>
        /// Gets true if errors are emitted else false.
        /// </summary>
        public bool HasErrors => _flowOutput.Errors.Count > 0;

        /// <summary>
        /// Gets true if messages are emitted else false.
        /// </summary>
        public bool HasMessages => _flowOutput.Messages.Count > 0;

        [Obsolete(null, true)]
        public bool HasOutputs => _flowOutput.Output.Count > 0;

        /// <summary>
        /// Gets cancellation token 
        /// </summary>
        public CancellationToken CancellationToken => _token;
    }
}

