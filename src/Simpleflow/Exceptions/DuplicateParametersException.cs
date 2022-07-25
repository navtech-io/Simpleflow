// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class DuplicateParametersException : SimpleflowException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public DuplicateParametersException(string message) : base(message) { }
    }
}
