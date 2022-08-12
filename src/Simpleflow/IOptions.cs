﻿// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// Gets or sets AllowFunctions
        /// </summary>
        public string[] AllowFunctions { get; set; }


        /// <summary>
        /// Gets or sets DenyFunctions
        /// </summary>
        public string[] DenyFunctions { get; set; }

        /// <summary>
        /// Gets or sets cache options
        /// </summary>
        public CacheOptions CacheOptions { get; set; }
    }
}
