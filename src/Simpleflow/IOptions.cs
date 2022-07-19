// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// Gets or sets AllowArgumentToMutate
        /// </summary>
        bool AllowArgumentToMutate { get;  }

        /// <summary>
        /// Gets or sets AllowFunctions, 
        /// if nothing is specified all configured functions will be allowed.
        /// Either <see cref="AllowOnlyFunctions"/> or <see cref="DenyOnlyFunctions"/> 
        /// must be set, not both.
        /// </summary>
        public string[] AllowOnlyFunctions { get; set; }


        /// <summary>
        /// Gets or sets DenyFunctions.
        /// Either <see cref="AllowOnlyFunctions"/> or <see cref="DenyOnlyFunctions"/> 
        /// must be set, not both.
        /// </summary>
        public string[] DenyOnlyFunctions { get; set; }
    }
}
