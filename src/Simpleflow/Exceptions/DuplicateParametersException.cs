﻿// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    public class DuplicateParametersException : SimpleflowException
    {
        // TODO

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DuplicateParametersException(string message) : base(message) { }
    }
}
