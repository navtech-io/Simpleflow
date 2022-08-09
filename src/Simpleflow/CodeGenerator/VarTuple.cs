// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.CodeGenerator
{
    internal class VarTuple<T>
    {
        public readonly T Value;
        public readonly Exception Error;

        public VarTuple(T value, Exception error)
        {
            Value = value;
            Error = error;
        }
    }

    internal class VarTuple
    {
        // Don't remove this field, it will be useful to get value from try catch expression without if
        public readonly object Value; 
        public readonly Exception Error;

        public VarTuple(Exception error)
        {
            Error = error;
        }
    }

}
