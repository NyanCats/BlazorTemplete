using System;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorTemplate.Server.Infrastructures.Repositories
{
    public class InMemoryRepository<Tkey, Titem>
    {
        const double compactionPercentage = 0.2d;
        public int TimeOut { get; protected set; } = 3600;

        private static MemoryCache Cache { get; set; } = new MemoryCache
        (
            new MemoryCacheOptions()
            {
                CompactionPercentage = compactionPercentage
            }
        );

        public InMemoryRepository(int timeOutSec)
        {
            TimeOut = timeOutSec;
        }

        public void Add(Tkey key, Titem item)
        {
            if (Contains(key)) return;

            Cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(TimeOut);
                entry.AbsoluteExpiration = null;
                return 1;
            });
        }

        public int Find(Tkey key)
        {
            Cache.TryGetValue(key, out int count);
            return count;
        }

        public void Update(Tkey key, Titem count)
        {
            if (!Contains(key)) return;
            Cache.Set(key, count, TimeSpan.FromSeconds(TimeOut));
        }

        public bool Contains(Tkey key)
        {
            return Cache.TryGetValue(key, out int count);
        }

        public void Remove(Tkey key)
        {
            Cache.Remove(key);
        }
    }
}
