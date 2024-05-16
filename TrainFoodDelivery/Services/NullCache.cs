using Microsoft.Extensions.Caching.Distributed;

namespace TrainFoodDelivery.Services;

public class NullCache : IDistributedCache
{
    public byte[] Get(string key)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetAsync(string key, CancellationToken token = default)
    {
        return Task.FromResult<byte[]>(Array.Empty<byte>());
    }

    public void Refresh(string key)
    {
        throw new NotImplementedException();
    }

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}
