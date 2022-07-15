// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxException : SimpleflowException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public SyntaxException(string message) : base(message)
        {
        }
    }
}
