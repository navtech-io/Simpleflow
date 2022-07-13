// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{

    public class VariableNameViolationException : Exception
    {
        public VariableNameViolationException(string name) 
            : base(String.Format(Resources.Message.ReservedWordException, name) ) { }
    }
}
