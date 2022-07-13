// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    public class ValueTypeMismatchException : Exception
    {
        public ValueTypeMismatchException(string message) : base(message) { }

        public ValueTypeMismatchException(string value, string expectedType) : base($"Invalid value '{value}'. Expected value type is {expectedType}") { }
    }
}
