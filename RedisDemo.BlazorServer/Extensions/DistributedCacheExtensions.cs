using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisDemo.BlazorServer.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordIdKey, T dataValue,
        TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime
        };

        var jsonData = JsonSerializer.Serialize(dataValue);

        await cache.SetStringAsync(recordIdKey, jsonData, options);
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordIdKey)
    {
        var jsonData = await cache.GetStringAsync(recordIdKey);

        if (jsonData is null)
            return default(T)!;

        return JsonSerializer.Deserialize<T>(jsonData)!;
    }
}
