// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when function name is not specified as per naming standards.
    /// </summary>
    public class InvalidFunctionNameException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFunctionNameException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="functionName">The function name that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public InvalidFunctionNameException(string functionName, string message=null) : base(message ?? $"Invalid function name '{functionName}'")
        {
            FunctionName = functionName;
        }

        /// <summary>
        /// Gets the name of the function that caused the error.
        /// </summary>
        public string FunctionName { get; }
    }
}
