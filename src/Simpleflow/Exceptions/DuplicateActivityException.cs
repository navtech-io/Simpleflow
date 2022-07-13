// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class DuplicateActivityException : Exception
    {
        // TODO

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DuplicateActivityException(string name) : base($"Activity '{name}' cannot be duplicated.") {}
    }
}
