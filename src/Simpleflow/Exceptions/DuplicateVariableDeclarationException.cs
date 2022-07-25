// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// The exception is thrown when a same variable is defined more than once.
    /// </summary>
    public class DuplicateVariableDeclarationException : SimpleflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateVariableDeclarationException"/> class with
        /// a specified variable name.
        /// </summary>
        /// <param name="name">The variable name that caused the exception.</param>
        public DuplicateVariableDeclarationException(string name) 
            : base(string.Format(Resources.Message.VariableAlreadyDefined, name))
        {
            VariableName = name;
        }

        /// <summary>
        /// Gets name of the variable that has been declared more than once.
        /// </summary>
        public string VariableName { get; }
    }
}
