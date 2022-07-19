// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Simpleflow.Services;

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public class FlowContextOptions : FlowOptions, IContextOptions
    {
        /// <summary>
        /// Gets or sets unique id of the script
        /// If Id is supplied, <see cref="CacheService"/> will use it to identify the compiled object
        /// in cache otherwise it creates a hash id for that script.
        /// </summary>
        public string Id { get; set; }
        
    }
}
