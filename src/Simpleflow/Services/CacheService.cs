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

        /// <summary>
        /// 
        /// </summary>
        public CacheService()
        {
            //MemoryCache
            _cache = new MemoryCache( new MemoryCacheOptions() { });
        }

        /// <inheritdoc />
        public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
        {
            // Create unique id for script to identify in cache store
            var id = GetScriptUniqueId(context.Script);
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
                           options: new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));

                context.Trace.Write($"Saved into cache {id} - Succeeded");
            }
        }

        protected virtual string GetScriptUniqueId(string script)
        {
            // Calculate id for script
            using var sha1 = SHA1.Create();
            return System.Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(script)));
        }
    }

}
