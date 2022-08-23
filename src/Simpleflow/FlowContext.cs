// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow
{
    /// <summary>
    /// A <see cref="FlowContext&lt;TArg&gt;"/> instance represents input and output.
    /// </summary>
    public sealed class FlowContext<TArg>
    {
        public string Script { get; set; }
    
        public TArg Argument { get; set; }

        public IFunctionRegister FunctionRegister { get; set; }

        public IContextOptions Options { get; set; }

        public FlowOutput Output { get; } = new FlowOutput(); 
        
        /// <summary>
        /// Gets the trace to verify the middleware info
        /// </summary>
        public SimpleflowTrace Trace { get; private set; }
    
        public FlowInternals Internals { get; } = new FlowInternals();

        public void EnableTrace()
        {
            Trace = new SimpleflowTrace();
        }

        public class FlowInternals
        {
            public Action<TArg, FlowOutput, RuntimeContext> CompiledScript { get; set; }
        }
    }
}
