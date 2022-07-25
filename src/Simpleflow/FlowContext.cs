// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Simpleflow
{
    /// <summary>
    /// A <see cref="FlowContext"/> instance represents input, output, and which has
    /// special property "Items" can be used by middleware to share data among.
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
        /// Gets trace to verify the executed middleware
        /// </summary>
        public SimpleflowTrace Trace { get; } = new SimpleflowTrace();

        /// <summary>
        /// 
        /// </summary>
        public FlowInternals Internals { get; } = new FlowInternals();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetScriptHash()
        {
            // TODO
            return "";
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

            // Maintain flags by scanning the script in visitors classes
            // HasMutates 
            // HasActivities
            // List<Activities> ActivitiesUsed

            /// <summary>
            /// 
            /// </summary>
            public dynamic Tag { get; } = new ExpandoObject();

           
        }
    }
}
