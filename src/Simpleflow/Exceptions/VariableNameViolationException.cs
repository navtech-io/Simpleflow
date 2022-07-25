// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when a keyword is used for a variable.
    /// </summary>
    public class VariableNameViolationException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNameViolationException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="name">The name that caused the exception.</param>
        /// <param name="message">The message describes the exception.</param>
        public VariableNameViolationException(string name, string message=null) 
            : base(message ?? String.Format(Resources.Message.ReservedWordException, name) ) { }
    }
}
