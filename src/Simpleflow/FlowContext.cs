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
        /// <summary>
        /// Gets or initializes input.
        /// The purpose of init access modifier to retain the input as it is by preventing the changes
        /// by middleware
        /// </summary>
        //public FlowInput Input { get; init; }  

        public string Script { get; set; }
    
        /// There can be one argument can be passed and used as context in script
        public TArg Argument { get; set; }

        /// 
        public IFunctionRegister FunctionRegister { get; set; }

        /// 
        public IContextOptions Options { get; set; }

        /// <summary>
        /// Gets or sets output.
        /// </summary>
        public FlowOutput Output { get; } = new FlowOutput(); 
        
        /// <summary>
        /// Gets trace to verify the middleware info
        /// </summary>
        public SimpleflowTrace Trace { get; private set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public FlowInternals Internals { get; } = new FlowInternals();

        /// <summary>
        /// 
        /// </summary>
        public void EnableTrace()
        {
            Trace = new SimpleflowTrace();
        }



        /// <summary>
        /// 
        /// </summary>
        public class FlowInternals
        {
            /// <summary>
            /// 
            /// </summary>
            public Action<FlowInput<TArg>, FlowOutput, ScriptHelperContext> CompiledScript { get; set; }
        }
    }
}
