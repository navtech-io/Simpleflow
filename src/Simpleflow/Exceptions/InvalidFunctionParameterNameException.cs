// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.


namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when function parameter is not valid type or not declared.
    /// </summary>
    public class InvalidFunctionParameterNameException : SimpleflowException
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidFunctionParameterNameException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFunctionException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="parameterName">The message that describes the error.</param>
        /// <param name="functionName"></param>
        public InvalidFunctionParameterNameException(string parameterName, string functionName)
            : base($"Function '{functionName}' does not have parameter '{parameterName}'") { }
    }
}
