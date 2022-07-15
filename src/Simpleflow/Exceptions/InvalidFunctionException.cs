// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidFunctionException : SimpleflowException
    {
        
        public InvalidFunctionException(string functionName) : base($"Invalid function '{functionName}'")
        {
            // unregistered function
        }
    }
}
