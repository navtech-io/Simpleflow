// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Collections.Generic;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when same parameters are specified multiple times
    /// while invoking a function in script
    /// </summary>
    public class DuplicateParametersException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateParametersException"/> class with
        /// a specified function name.
        /// </summary>
        /// <param name="repeatedParameters">The repeated parameters that caused the exception.</param>
        public DuplicateParametersException(IEnumerable<string> repeatedParameters) : base($"Duplicate parameters '{string.Join(',', repeatedParameters)}' cannot be allowed." ) { }
    }
}
