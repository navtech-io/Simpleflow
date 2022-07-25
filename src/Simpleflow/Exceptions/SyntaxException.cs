// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Collections.Generic;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when any syntax errors are present in the Simpleflow script.
    /// </summary>
    public class SyntaxException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleflowException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errors">The list of syntax errors that caused the exception.</param>
        public SyntaxException(string message, IEnumerable<SyntaxError> errors) : base(message)
        {
            Errors = errors;
        }

        IEnumerable<SyntaxError> Errors { get; }
    }
}
