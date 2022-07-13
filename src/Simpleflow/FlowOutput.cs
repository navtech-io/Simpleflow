// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;

namespace Simpleflow
{
    /// <summary>
    /// Represents final output of simple flow
    /// </summary>
    public sealed class FlowOutput
    {
        /// <summary> 
        /// Get or set messages
        /// </summary>
        public List<string> Messages { get; } = new List<string>();

        /// <summary>
        /// if there are errors engines throws an exception ValidationException 
        /// </summary>
        public List<string> Errors { get; } = new List<string>();

        /// <summary>
        /// Get or set arbitrary objects as output from script
        /// </summary>
        public Dictionary<string, object> Output { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }
}

