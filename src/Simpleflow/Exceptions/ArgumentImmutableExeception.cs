// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// Represents base exception for all Simpleflow exceptions
    /// </summary>
    public class ArgumentImmutableExeception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleflowException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ArgumentImmutableExeception(): base($"Script argument cannot be mutable in this context. Set {nameof(FlowOptions.AllowArgumentToMutate)} options to true in order to modify argument")
        {

        }
    }
}
