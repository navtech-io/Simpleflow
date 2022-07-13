// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// Represents input object to simple flow
    /// </summary>
    public class FlowInput<TArg>
    {

        public FlowInput(TArg argument, IOptions options=null)
        {
            Argument = argument;
            Options = options;
        }

        /// There can be one argument can be passed and used as context in script
        public TArg Argument { get; }

        // public IActivityConfig ActivityConfig { get; set; }
        /// 
        public IOptions Options { get; }
    }
}
