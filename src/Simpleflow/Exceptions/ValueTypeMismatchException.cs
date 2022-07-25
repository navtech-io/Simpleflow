// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when different type of value is specified than initially declared type.
    /// </summary>
    public class ValueTypeMismatchException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypeMismatchException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ValueTypeMismatchException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypeMismatchException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="value">The value that caused the exception.</param>
        /// <param name="expectedType">The expected data type of value.</param>
        public ValueTypeMismatchException(string value, string expectedType) : base($"Invalid value '{value}'. Expected type of value is {expectedType}") { }

    }
}
