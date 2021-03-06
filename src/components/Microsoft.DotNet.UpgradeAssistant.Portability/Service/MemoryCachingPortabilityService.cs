﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.DotNet.UpgradeAssistant.Portability.Service
{
    public class MemoryCachingPortabilityService : IPortabilityService
    {
        private readonly IPortabilityService _other;
        private readonly Dictionary<string, ApiInformation> _cache;

        public MemoryCachingPortabilityService(IPortabilityService other)
        {
            _other = other;
            _cache = new Dictionary<string, ApiInformation>(StringComparer.Ordinal);
        }

        public async IAsyncEnumerable<ApiInformation> GetApiInformation(IReadOnlyCollection<string> apis, [EnumeratorCancellation] CancellationToken token)
        {
            if (apis is null)
            {
                throw new ArgumentNullException(nameof(apis));
            }

            var nonCachedApis = new List<string>();

            foreach (var api in apis)
            {
                if (_cache.TryGetValue(api, out var known))
                {
                    yield return known;
                }
                else
                {
                    nonCachedApis.Add(api);
                }
            }

            await foreach (var api in _other.GetApiInformation(nonCachedApis, token))
            {
                _cache.Add(api.Definition.DocId, api);
                yield return api;
            }
        }
    }
}
