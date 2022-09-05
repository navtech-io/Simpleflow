// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// Use to provide appropriate delegate based on name 
    /// and argument information
    /// </summary>
    public interface IFunctionProvider
    {
        /// <summary>
        /// Get function reference to invoke
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argumentInfo"></param>
        /// <returns></returns>
        FunctionPointer GetFunction(string name, ArgumentInfo[] argumentInfo);
    }
}
