using System;
using System.Collections.Generic;

namespace Pkgdef_CSharp
{
    internal class VersionedCache<SourceType,VersionType,CacheValueType>
    {
        private readonly IDictionary<SourceType, Tuple<VersionType, CacheValueType>> cachedValues;

        public VersionedCache()
        {
            this.cachedValues = new Dictionary<SourceType, Tuple<VersionType, CacheValueType>>();
        }

        public bool TryGet(SourceType source, VersionType version, out CacheValueType cachedValue)
        {
            PreCondition.AssertNotNull(source, nameof(source));
            PreCondition.AssertNotNull(version, nameof(version));

            bool result = false;
            cachedValue = default;

            if (this.cachedValues.TryGetValue(source, out Tuple<VersionType, CacheValueType> cachedValueWithVersion) &&
                object.Equals(cachedValueWithVersion.Item1, version))
            {
                result = true;
                cachedValue = cachedValueWithVersion.Item2;
            }

            return result;
        }

        public void Set(SourceType source, VersionType version, CacheValueType cachedValue)
        {
            this.cachedValues[source] = Tuple.Create(version, cachedValue);
        }
    }
}
