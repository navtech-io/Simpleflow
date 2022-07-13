// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <inheritdoc />
    public class FlowOptions : IOptions
    {
        /// <inheritdoc />
        public bool ReadOnlyExecution { get; set; }

        /// <inheritdoc />
        public bool RunFunctions { get; set; }

        /// <inheritdoc />
        public string[] PermitFunctions { get; set; }
    }
}
