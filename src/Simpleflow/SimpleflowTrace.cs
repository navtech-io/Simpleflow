// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Text;

namespace Simpleflow
{
    /// <summary>
    /// Represents trace log
    /// </summary>
    public sealed class SimpleflowTrace
    {
        private readonly StringBuilder _servicesTrace;
        private readonly StringBuilder _logTrace;

        /*  Stack Trace
          readonly LinkedList<Point>  list = new LinkedList<Point>(); */

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleflowTrace"/>
        /// </summary>
        public SimpleflowTrace()
        {

            _servicesTrace = new StringBuilder();
            _logTrace = new StringBuilder();
        }

        /// <summary>
        /// Adds service to trace, internal purpose only for implementing ISimpleflow 
        /// </summary>
        /// <param name="serviceName"></param>
        public void CreateNewTracePoint(string serviceName)
        {
            _servicesTrace.AppendLine(serviceName);
        }

        /// <summary>
        /// Writes message to log trace 
        /// </summary>
        /// <param name="message">Message to write</param>
        public void Write(string message)
        {
            _logTrace.AppendLine(message);
        }

        /// <summary>
        /// Gets logs of trace
        /// </summary>
        /// <returns></returns>
        public string GetLogs()
        {
            return _logTrace.ToString();
        }

        /// <summary>
        /// Gets services trace log
        /// </summary>
        /// <returns></returns>
        public override  string ToString()
        {
            return _servicesTrace.ToString();
        }
    }
}
