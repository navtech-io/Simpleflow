// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContextOptions : IOptions
    {
        /// <summary>
        /// Gets unique id of the script
        /// </summary>
        string Id { get; }
    }
}
