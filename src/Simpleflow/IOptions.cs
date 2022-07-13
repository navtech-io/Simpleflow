// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// Gets read-only execution setting
        /// <br/>Mutate and function execution will be ignored if ReadOnlyExecution is true
        /// </summary>
        bool ReadOnlyExecution { get;  }

        /// <summary>
        /// Gets indicator whether function will run or not
        /// </summary>
        bool RunFunctions { get; }

        /// <summary>
        /// Gets permitted functions, this property depends on RunActivities
        /// </summary>
        string[] PermitFunctions { get; }
    }
}
