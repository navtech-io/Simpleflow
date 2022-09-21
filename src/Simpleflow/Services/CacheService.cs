// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Text;
using System.Security.Cryptography;

#if NET48
using System.Runtime.Caching;
#else
using Microsoft.Extensions.Caching.Memory;
#endif


namespace Simpleflow.Services
{
    // Make thread safe
    /// <summary>
    /// A service to cache the generate code instructions
    /// </summary>
    public class CacheService : IFlowPipelineService
    {
#if NET48
        private readonly MemoryCache _cache;
#else
        private readonly IMemoryCache _cache;
#endif

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


#if NET48            
            //MemoryCache
            _cache = MemoryCache.Default;
#else
            _cache = new MemoryCache(new MemoryCacheOptions() { });
#endif

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

#if NET48
            _cache.Set(key: id,
                       value: context.Internals.CompiledScript,
                       policy: new CacheItemPolicy
                       {
                           AbsoluteExpiration = context.Options?.CacheOptions?.AbsoluteExpiration ?? _cacheOptions.AbsoluteExpiration ?? DateTimeOffset.MaxValue,
                           SlidingExpiration = context.Options?.CacheOptions?.SlidingExpiration ?? _cacheOptions.SlidingExpiration ?? CacheOptions.DefaultSlidingExpiration
                       });
#else
            _cache.Set(key: id,
                       value: context.Internals.CompiledScript,
                       options: new MemoryCacheEntryOptions
                       {
                           AbsoluteExpiration = context.Options?.CacheOptions?.AbsoluteExpiration ?? _cacheOptions.AbsoluteExpiration,
                           SlidingExpiration = context.Options?.CacheOptions?.SlidingExpiration ?? _cacheOptions.SlidingExpiration
                       });
#endif
            context.Trace?.Write($"Saved into cache {id} - Succeeded");
        }

        private bool GetAndSetToContextTheCompiledScript<TArg>(FlowContext<TArg> context, string id)
        {
#if NET48            
            var compiledScript = _cache.Get(key: id) as Action<TArg, FlowOutput, RuntimeContext>;
#else
            var compiledScript = _cache.Get<Action<TArg, FlowOutput, RuntimeContext>>(key: id);
#endif
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
