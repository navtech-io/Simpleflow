// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <inheritdoc />
    public class FlowOptions : IOptions
    {
        /// <inheritdoc />
        public bool AllowArgumentToMutate { get; set; }

        /// <inheritdoc />
        public string[] AllowFunctions { get; set; }

        /// <inheritdoc />
        public string[] DenyFunctions { get; set; }
    }
}
