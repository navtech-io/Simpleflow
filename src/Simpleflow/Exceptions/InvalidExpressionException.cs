// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidExpressionException : Exception
    {
        // TODO

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public InvalidExpressionException(string expression) : base($"'{expression}' is not valid.") { }
    }
}
