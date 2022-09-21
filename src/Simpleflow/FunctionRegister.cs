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
        // Key is name, and  value is index, an index represents block (type store/method store/function provider Store)
        // and index of store. 29th and 30th bit represent block/store, and rest of them as index (2**28) in block
        // block - 0 represents _methodStore
        // block - 1 represents _providerStore 
        // block - 2 represents ..unused..
        // block - 3 represents ..unused..

        private const int MethodStoreIndex = 0;
        private const int ProviderStoreIndex = 1;

        private readonly Dictionary<string, int> _bitmapIndex =
            new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        private readonly List<Delegate> _methodStore = new List<Delegate>(); 
        private readonly List<IFunctionProvider> _providerStore = new List<IFunctionProvider>(); 

        private readonly object _sync = new object();

        
        public IFunctionRegister Add(string name, Delegate @delegate)
        {
            ValidateFunctionName(name);

            //Allow only static methods
            if (@delegate.Target != null)
            {
                throw new SimpleflowException(Resources.Message.RegisterNonStaticMethodError);
            }

            if (_bitmapIndex.ContainsKey(name))
            {
                throw new DuplicateFunctionException(name);
            }

            int index;
            lock (_sync)
            {
                Debug.Assert(_bitmapIndex.Count == _methodStore.Count + _providerStore.Count);

                _methodStore.Add(@delegate);
                index = _methodStore.Count - 1;
            }
            _bitmapIndex.Add(name, CreateBitmapIndex(storeIndex: MethodStoreIndex, valueIndex: index));

            return this;
        }


        public IFunctionRegister Add(string name, IFunctionProvider functionProvider)
        {
            ValidateFunctionName(name);

            if (functionProvider == null)
            {
                throw new ArgumentNullException(nameof(functionProvider));
            }

            if (_bitmapIndex.ContainsKey(name))
            {
                throw new DuplicateFunctionException(name);
            }

            int index;
            lock (_sync)
            {
                Debug.Assert(_bitmapIndex.Count == _methodStore.Count + _providerStore.Count);

                _providerStore.Add(functionProvider);
                index = _providerStore.Count - 1;
            }
            _bitmapIndex.Add(name, CreateBitmapIndex(storeIndex: ProviderStoreIndex, valueIndex: index));

            return this;
        }


        /// <inheritdoc />
        public FunctionPointer GetFunction(string name, ArgumentInfo[] argumentInfo) // ArgumentInfo
        {
            if (_bitmapIndex.ContainsKey(name))
            {
                var bitmapIndex = _bitmapIndex[name];
                (int storeIndex, int valueIndex) = GetStoreAndValueIndex(bitmapIndex);

                // Change this code, in order to support .NET 48
                return storeIndex == MethodStoreIndex
                        ? new FunctionPointer { Reference = _methodStore[valueIndex] }
                        : storeIndex == ProviderStoreIndex
                          ? _providerStore[valueIndex].GetFunction(name, argumentInfo)
                          : null;

                //return storeIndex switch
                //{
                //    MethodStoreIndex   => new FunctionPointer { Reference = _methodStore[valueIndex] },
                //    ProviderStoreIndex => _providerStore[valueIndex].GetFunction(name, argumentInfo),
                //    _ => null
                //};

            }
            return null;
        }

        private (int storeIndex, int valueIndex) GetStoreAndValueIndex(int bitmapIndex)
        {
            var storeIndex = bitmapIndex >> 28;
            var valueIndex = (storeIndex << 28) ^ bitmapIndex;

            return (storeIndex, valueIndex);
        }

        private int CreateBitmapIndex(int storeIndex, int valueIndex)
        {
            return storeIndex << 28 | valueIndex;
        }

        private void ValidateFunctionName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var pattern = "^[_]*[a-zA-Z][_a-zA-Z0-9]*([.][_]*[a-zA-Z][_a-zA-Z0-9]*)*$";

            if (!System.Text.RegularExpressions.Regex.Match(name, pattern).Success)
            {
                throw new InvalidFunctionNameException(name);
            }
        }
    }
}
