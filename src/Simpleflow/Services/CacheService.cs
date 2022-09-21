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
            // Add trace for debugging
            context.Trace?.CreateNewTracePoint(nameof(CacheService));

            // Create unique id for script to identify in cache store
            var id = string.IsNullOrWhiteSpace(context.Options?.Id) ?
                            GetScriptUniqueId(context.Options?.CacheOptions, context.Script) : context.Options.Id;

            context.Trace?.Write($"Cache-Key {id}");

            // Get compiled script from cache and set it to context in order to avoid recompilation
            var isAvailableInCache = GetAndSetToContextTheCompiledScript(context, id);

            // Invoke next service in pipeline
            next?.Invoke(context);

            // Cache the compiled script
            if (!isAvailableInCache && context.Internals.CompiledScript != null)
            {
                StoreIntoCacheCompiledScript(context, id);
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
            using (var sha1 = HashAlgorithm.Create(contextCacheOptions?.HashingAlgToIdentifyScriptUniquely ?? _cacheOptions.HashingAlgToIdentifyScriptUniquely))
            {
                return System.Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(script)));
            }
        }


        private void StoreIntoCacheCompiledScript<TArg>(FlowContext<TArg> context, string id)
        {
            _cache.Set(key: id,
                                       value: context.Internals.CompiledScript,
                                       options: new MemoryCacheEntryOptions
                                       {
                                           AbsoluteExpiration = context.Options?.CacheOptions?.AbsoluteExpiration ?? _cacheOptions.AbsoluteExpiration,
                                           SlidingExpiration = context.Options?.CacheOptions?.SlidingExpiration ?? _cacheOptions.SlidingExpiration
                                       });

            context.Trace?.Write($"Saved into cache {id} - Succeeded");
        }

        private bool GetAndSetToContextTheCompiledScript<TArg>(FlowContext<TArg> context, string id)
        {
            var compiledScript = _cache.Get<Action<TArg, FlowOutput, RuntimeContext>>(key: id);
            var isAvailableInCache = compiledScript != null;

            if (isAvailableInCache)
            {
                if (context.Options?.ResetCache ?? false)
                {
                    _cache.Remove(key: id);
                    isAvailableInCache = false; // in order to save it back

                    context.Trace?.Write($"Reset cache entry '{id}' - Succeeded");
                }
                else
                {
                    context.Trace?.Write($"Read from cache {id} - Succeeded");
                    context.Internals.CompiledScript = compiledScript;
                }
            }

            return isAvailableInCache;
        }

    }

}
