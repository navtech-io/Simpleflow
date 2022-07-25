// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// Represents base exception for all Simpleflow exceptions
    /// </summary>
    public class SimpleflowException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleflowException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SimpleflowException(string message): base(message)
        {

        }
    }
}
