// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when a function with a name is already registered.
    /// </summary>
    public class DuplicateFunctionException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateFunctionException"/> class with
        /// a specified function name.
        /// </summary>
        /// <param name="functionName">The name of the function that caused the exception</param>
        /// <param name="message">The message describes the exception</param>
        public DuplicateFunctionException(string functionName, string message=null) : base(message ?? $"Function '{functionName}' is already registered, cannot be duplicated.") {
            FunctionName = functionName;
        }

        /// <summary>
        /// Gets name of the function that has been re-registered.
        /// </summary>
        public string FunctionName { get; }
    }
}
