using Microsoft.Extensions.Caching.Memory;

namespace RuyaOptik.Business.Services
{
    public class CacheVersionService
    {
        private readonly IMemoryCache _cache;

        public CacheVersionService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public int Get(string key)
        {
            if (!_cache.TryGetValue(key, out int version))
            {
                version = 1;
                _cache.Set(key, version);
            }

            return version;
        }

        public int Increment(string key)
        {
            var current = Get(key);
            var next = current + 1;
            _cache.Set(key, next);
            return next;
        }
    }
}
