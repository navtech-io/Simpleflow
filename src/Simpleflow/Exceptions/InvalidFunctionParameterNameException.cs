// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    public class InvalidFunctionParameterNameException : SimpleflowException
    {
        // TODO

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public InvalidFunctionParameterNameException(string message) : base(message) { }
    }
}
