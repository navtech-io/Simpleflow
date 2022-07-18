// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Collections.Generic;

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
        /// <param name="errors"></param>
        public SyntaxException(string message, IEnumerable<SyntaxError> errors) : base(message)
        {
            Errors = errors;
        }

        IEnumerable<SyntaxError> Errors { get; }
    }
}
