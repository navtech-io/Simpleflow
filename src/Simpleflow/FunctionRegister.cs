// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Simpleflow.Exceptions;

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FunctionRegister : IFunctionRegister // ,IActivityInvoker
    {
        // Key is name, and  value is index, an index represents block (type store/method store)
        // and index of store. 30th bit represents block, and rest of them as index (2**29) in block
        // block - 1 represents _typeStore and 0 represents _methodStore

        private readonly Dictionary<string, int> _bitmapIndex =
            new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        private readonly List<Type> _typeStore = new List<Type>();
        private readonly List<Delegate> _methodStore = new List<Delegate>();

        private readonly object _sync = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        /// 
        // Need to enable this later once this feature is implemented
        private IFunctionRegister Add(string name, Type activity)
        {
            // TODO : able to invoke methods of the Type 
            // find all methods and add it
            // let customer = $customer.new()
            // $MethodName (p1: value, ...) on customer

            ValidateFunctionName(name);

            if (_bitmapIndex.ContainsKey(name))
            {
                throw new DuplicateFunctionException(name);
            }

            int index;
            lock (_sync)
            {
                Debug.Assert(_bitmapIndex.Count == _typeStore.Count + _methodStore.Count);

                _typeStore.Add(activity);
                index = _typeStore.Count - 1;
            }

            _bitmapIndex.Add(name, 1 << 29 | index);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public IFunctionRegister Add(string name, Delegate @delegate)
        {
            //Allow only static methods
            if (@delegate.Target != null)
            {
                throw new SimpleflowException(Resources.Message.RegisterNonStaticMethodError);
            }

            ValidateFunctionName(name);

            if (_bitmapIndex.ContainsKey(name))
            {
                throw new DuplicateFunctionException(name);
            }

            int index;
            lock (_sync)
            {
                Debug.Assert(_bitmapIndex.Count == _typeStore.Count + _methodStore.Count);

                _methodStore.Add(@delegate);
                index = _methodStore.Count - 1;
            }
            // 0 << 29 | index , since 0 << 29 is 0,
            // so here we don't need to use bitwise operation to add up together
            _bitmapIndex.Add(name, index);

            return this;
        }

        /// <inheritdoc />
        public Delegate GetFunction(string name)
        {
            if (_bitmapIndex.ContainsKey(name))
            {
                var bitmapIndex = _bitmapIndex[name];

                var firstBit = bitmapIndex >> 29;
                var index = (firstBit << 29) ^ bitmapIndex;

                // ReSharper disable once InconsistentlySynchronizedField
                return firstBit == 1 ? null : _methodStore[index];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        private bool? IsFunctionAvailableInClass(string name)
        {
            if (_bitmapIndex.ContainsKey(name))
            {
                var bitmapIndex = _bitmapIndex[name];
                var firstBit = bitmapIndex >> 29;

                return firstBit == 1;
            }
            return false;
        }

        private void ValidateFunctionName(string name)
        {
           var pattern = "^[_]*[a-zA-Z][_a-zA-Z0-9]*([.][_]*[a-zA-Z][_a-zA-Z0-9]*)*$";

            if (!System.Text.RegularExpressions.Regex.Match(name,pattern).Success)
            {
                throw new InvalidFunctionNameException(name);
            }
        }

    }
}
