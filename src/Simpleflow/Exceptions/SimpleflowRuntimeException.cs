// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;

namespace Simpleflow.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleflowRuntimeException : SimpleflowException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lineNumber"></param>
        /// <param name="statement"></param>
        /// <param name="innerException"></param>
        public SimpleflowRuntimeException(string message, int lineNumber, string statement, Exception innerException) : base(message, innerException)
        {
            LineNumber = lineNumber;
            Statement = statement;
        }

        /// <summary>
        /// Gets line number where error has occurred
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// Gets statement where error has occurred
        /// </summary>
        public string Statement { get; }
    }
}
