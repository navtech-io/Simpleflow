// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.


namespace Simpleflow
{
    /// <inheritdoc />
    public class FlowOptions : IOptions
    {
        /// <summary>
        /// Gets or sets AllowArgumentToMutate
        /// </summary>
        public bool AllowArgumentToMutate { get; set; }

        /// <summary>
        /// Gets or sets AllowFunctions
        /// </summary>
        public string[] AllowFunctions { get; set; }

        /// <summary>
        /// Gets or sets DenyFunctions
        /// </summary>
        public string[] DenyFunctions { get; set; }
    }
}
