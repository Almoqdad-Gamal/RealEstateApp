using System.Text.Json;
using RealEstateApp.Application.Interfaces;
using StackExchange.Redis;

namespace RealEstateApp.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _redis;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(5);
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);

            if(value.IsNullOrEmpty)
                return default ;
            
            // Convert the json string to an object
            return JsonSerializer.Deserialize<T>((string)value!);

        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            // Convert the object to a json string
            var jsonValue = JsonSerializer.Serialize(value);

            await _database.StringSetAsync(key, jsonValue, expiration ?? _defaultExpiration);
        }
        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            // Get all the keys that starts with this prefix
            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoints.First());

            var keys = new List<RedisKey>();

            await foreach(var key in server.KeysAsync(pattern: $"{prefix}*"))
            {
                keys.Add(key);
            }

            if(keys.Any())
                await _database.KeyDeleteAsync(keys.ToArray());
        }

    }
}