// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class DuplicateFunctionException : SimpleflowException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DuplicateFunctionException(string name) : base($"Function '{name}' is already registered, cannot be duplicated.") {
            FunctionName = name;
        }

        /// <summary>
        /// Gets name of the function that has been duplicated.
        /// </summary>
        public string FunctionName { get; }
    }
}
