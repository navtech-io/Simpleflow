// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

namespace Simpleflow.Exceptions
{

    /// <summary>
    /// This exception is thrown when a script uses a denied function.
    /// </summary>
    public class AccessDeniedException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessDeniedException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public AccessDeniedException(string message) : base(message)
        {
        }
    }
}
