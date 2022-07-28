// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Threading;

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContextOptions : IOptions
    {
        /// <summary>
        /// Gets or sets unique id of the script
        /// If Id is supplied, CacheService will use it to identify the compiled object
        /// in cache otherwise it creates a hash id for that script.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Reset cache allows to remove the item from cache if exists and add it once its compiled.
        /// </summary>
        public bool ResetCache { get; set; }

        /// <summary>
        /// Gets or sets <see cref="System.Threading.CancellationToken"/>
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
