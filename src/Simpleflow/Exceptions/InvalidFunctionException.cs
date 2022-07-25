// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.


namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when function name is not registered with the Simpleflow runtime.
    /// Please use <see cref="FunctionRegister"/> or <see cref="IFunctionRegister"/> appropriately to register functions.
    /// </summary>
    public class InvalidFunctionException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFunctionException"/> class with
        /// a specified function name.
        /// </summary>
        /// <param name="functionName">The function name that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public InvalidFunctionException(string functionName, string message=null) : base(message ?? $"Invalid function '{functionName}'")
        {
            FunctionName = functionName;
        }

        /// <summary>
        /// Gets the name of the function that caused the error.
        /// </summary>
        public string FunctionName { get; }
    }
}
