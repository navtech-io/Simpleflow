﻿// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    public class InvalidFunctionNameException : SimpleflowException
    {
        public InvalidFunctionNameException(string functionName) : base($"Invalid function name '{functionName}'")
        {
            FunctionName = functionName;
        }

        public string FunctionName { get; }
    }
}