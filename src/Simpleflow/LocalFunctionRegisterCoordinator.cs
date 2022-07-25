// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;

namespace Simpleflow
{
    internal class FunctionRegisterCoordinator : IFunctionRegister
    {
        private readonly IFunctionRegister _globalFunctionRegister;
        private readonly IFunctionRegister _contextFunctionRegister;

        public FunctionRegisterCoordinator(IFunctionRegister globalRegister, IFunctionRegister contextRegister = null)
        {
            _globalFunctionRegister = globalRegister ?? throw new ArgumentNullException(nameof(globalRegister));
            _contextFunctionRegister = contextRegister;
        }

        public IFunctionRegister Add(string name, Delegate @delegate)
        {
            throw new NotImplementedException();
        }

        public Delegate GetFunction(string name)
        {
            return _contextFunctionRegister?.GetFunction(name) ??
                    _globalFunctionRegister.GetFunction(name);
        }
    }
}
