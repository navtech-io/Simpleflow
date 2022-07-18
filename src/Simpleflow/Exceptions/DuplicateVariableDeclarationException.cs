// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class DuplicateVariableDeclarationException : SimpleflowException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DuplicateVariableDeclarationException(string name) 
            : base(string.Format(Resources.Message.VariableAlreadyDefined, name))
        {
            VariableName = name;
        }

        /// <summary>
        /// Gets name of the variable that has been declared more than once
        /// </summary>
        public string VariableName { get; }
    }
}
