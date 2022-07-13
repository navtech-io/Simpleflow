// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// Defines a contract to execute simple flow
    /// </summary>
    public interface ISimpleflow
    {
        
        //FlowOutput Run(FlowInput flowInput);

        FlowOutput Run<TArg>(string script, TArg argument);
        FlowOutput Run<TArg>(string script, TArg argument, IOptions options);
        FlowOutput Run<TArg>(string script, TArg argument, IFunctionRegister config);
        FlowOutput Run<TArg>(string script, TArg argument, IOptions options, IFunctionRegister config);
    }
}
