// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    /// <summary>
    /// Represents the cache options that applied to <see cref="Services.CacheService"/>
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public System.TimeSpan? SlidingExpiration { get; set; } = System.TimeSpan.FromMinutes(3);

        /// <summary>
        /// Gets or sets an absolute expiration date for the cache entry.
        /// </summary>
        public System.DateTimeOffset? AbsoluteExpiration { get; set; }

        /// <summary>
        /// Gets or sets hashing algorithm to identify script compiled object uniquely
        /// Check available hash algorithms and names here: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.create?view=net-6.0
        /// </summary>
        public string HashingAlgToIdentifyScriptUniquely { get; set; } = "MD5";
    }
}
