namespace Manufacturing.Api.State;

using System;
using System.Runtime.Caching;
using System.Threading;
using Sqids;
using MemoryCache = System.Runtime.Caching.MemoryCache;

public class GameStateManager
{
    private int _id;
    private static readonly ObjectCache GameCache = MemoryCache.Default;
    private static readonly CacheItemPolicy CacheItemPolicy = new()
    {
        AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) // Set cache expiration to 1 hour
    };
    private static readonly SqidsEncoder<int> Sqids = new();
    
    public void AddGame(string key, GameState gameState)
    {
        GameCache.Set(key, gameState, CacheItemPolicy);
    }

    public GameState GetGame(string key)
    {
        return GameCache.Get(key) as GameState;
    }

    public bool RemoveGame(string key)
    {
        return GameCache.Remove(key) != null;
    }

    public bool GameExists(string key)
    {
        return GameCache.Contains(key);
    }
    
    //  return the next ID and increment the current ID
    public string NextIdAndIncrement => Sqids.Encode(Interlocked.Increment(ref _id));
}