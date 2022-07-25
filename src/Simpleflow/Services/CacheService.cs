// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;

namespace Simpleflow.Services
{
    // Make thread safe
    /// <summary>
    /// A service to cache the generate code instructions
    /// </summary>
    public class CacheService : IFlowPipelineService
    {
        private readonly IMemoryCache _cache;
        private readonly CacheOptions _cacheOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheOptions">
        /// </param>
        public CacheService(CacheOptions cacheOptions)
        {
            _cacheOptions = cacheOptions ?? throw new ArgumentNullException(nameof(cacheOptions)); 

            // validate hashing algorithm for unique id generation
            if (string.IsNullOrWhiteSpace(cacheOptions.HashingAlgToIdentifyScriptUniquely))
            {
                throw new ArgumentNullException(nameof(cacheOptions.HashingAlgToIdentifyScriptUniquely));
            }

            //MemoryCache
            _cache = new MemoryCache(new MemoryCacheOptions() { });

        }

        /// <summary>
        /// 
        /// </summary>
        public CacheService() : this(new CacheOptions())
        {
        }
        
        /// <inheritdoc />
        public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
        {
            // Create unique id for script to identify in cache store
            var id = string.IsNullOrWhiteSpace(context.Options?.Id) ?  
                            GetScriptUniqueId(context.Options?.CacheOptions, context.Script) : context.Options.Id;

            // GetFlowContextOptionsId helps to identify script uniquely along with options
            // in order to allow or deny functions 
            if (context.Options != null)
            {
                id += "_" + GetFlowContextOptionsId(context.Options);
            }

            context.Trace.Write($"Cache-Key {id}");

            // Get compiled script from cache
            var compiledScript = _cache.Get< Action<FlowInput<TArg>, FlowOutput, ScriptHelperContext>>(key: id);
            if (compiledScript != null)
            {
                context.Trace.Write($"Read from cache {id} - Succeeded");
                context.Internals.CompiledScript = compiledScript;
            }

            // Invoke next service in pipeline
            next?.Invoke(context);

            // Cache compiled script
            if (compiledScript == null && context.Internals.CompiledScript != null)
            {
                _cache.Set(key: id,
                           value: context.Internals.CompiledScript,
                           options: new MemoryCacheEntryOptions { 
                                AbsoluteExpiration = context.Options?.CacheOptions?.AbsoluteExpiration ?? _cacheOptions.AbsoluteExpiration,
                                SlidingExpiration = context.Options?.CacheOptions?.SlidingExpiration ??  _cacheOptions.SlidingExpiration
                           }); 

                context.Trace.Write($"Saved into cache {id} - Succeeded");
            }
        }

        /// <summary>
        /// Gets script unique id by creating hash (SHA256) for the input script
        /// </summary>
        /// <param name="contextCacheOptions"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        protected virtual string GetScriptUniqueId(CacheOptions contextCacheOptions, string script)
        {
            // Calculate id for script
            using var sha1 = HashAlgorithm.Create(contextCacheOptions?.HashingAlgToIdentifyScriptUniquely ?? _cacheOptions.HashingAlgToIdentifyScriptUniquely);
            return System.Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(script)));
        }

        private string GetFlowContextOptionsId(IContextOptions options)
        {
            if (
                //options.AllowArgumentToMutate == false &&
                (options.AllowFunctions == null || options.AllowFunctions.Length == 0)
                && (options.DenyFunctions == null  || options.DenyFunctions.Length == 0)
                )
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            //sb.Append(string.Join(' ', options.AllowArgumentToMutate));

            if (options.AllowFunctions != null && options.AllowFunctions.Length > 0)
            {
                sb.Append("Allow"); //ensure add this to avoid collisions
                sb.Append(string.Join(' ', options.AllowFunctions));
            }

            if (options.DenyFunctions != null && options.DenyFunctions.Length > 0)
            {
                sb.Append("Deny"); //ensure add this to avoid collisions
                sb.Append(string.Join(' ', options.DenyFunctions ));
            }

            return GetScriptUniqueId(options.CacheOptions, sb.ToString());
        }
    }

}
