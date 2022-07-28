// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Threading;
using Simpleflow.Services;

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public class FlowContextOptions : FlowOptions, IContextOptions
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public bool ResetCache { get; set; }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; }
    }
}
