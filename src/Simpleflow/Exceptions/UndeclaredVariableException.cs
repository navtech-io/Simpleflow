// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when a variable is used that has not been declared in the script.
    /// </summary>
    public class UndeclaredVariableException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndeclaredVariableException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="name">The name of the variable that caused the exception.</param>
        public UndeclaredVariableException(string name, string message=null) : base(message ?? $"Variable '{name}' is not declared.")
        {
            VariableName = name;
        }

        /// <summary>
        /// Gets the name of the variable that has not been declared in script.
        /// </summary>
        public string VariableName { get; }
    }
}
