// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when unavailable property is specified to access.
    /// </summary>
    public class InvalidPropertyException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPropertyException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidPropertyException(string message): base(message)
        {

        }
    }
}
