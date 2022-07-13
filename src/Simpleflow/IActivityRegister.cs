// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFunctionRegister
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        // IFunctionRegister Add(string name, Type activity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="delegate"></param>
        /// <returns></returns>
        IFunctionRegister Add(string name, Delegate @delegate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Delegate GetFunction(string name);
    }
}
