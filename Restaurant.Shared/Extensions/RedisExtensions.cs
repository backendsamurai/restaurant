using System.Linq.Expressions;
using Mapster;
using Redis.OM;
using Redis.OM.Searching;

namespace Restaurant.Shared.Extensions;

public static class RedisExtensions
{
    public static List<TOut> GetOrSet<TIn, TOut>(this IRedisCollection<TIn>? collection, Func<List<TOut>> setter)
    {
        if (collection is not null && collection.Any())
            return collection.Adapt<List<TOut>>();

        var items = setter();

        if (items.Count == 0) return [];

        foreach (var item in items)
            collection?.Insert(item.Adapt<TIn>());

        return items;
    }

    public static List<TOut> GetOrSet<TIn, TOut>(this IRedisCollection<TIn>? collection, Expression<Func<TIn, bool>> expression, Func<List<TOut>> setter) where TIn : class
    {
        if (collection is null) return [];

        var cachedItems = collection.Where(expression).ToList();

        if (cachedItems.Count > 0)
            return cachedItems.Adapt<List<TOut>>();

        var items = setter();

        if (items.Count <= 0) return [];

        foreach (var item in items)
            collection.InsertAsync(item.Adapt<TIn>());

        return [];
    }

    public static TOut? GetOrSet<TIn, TOut>(this IRedisCollection<TIn>? collection, Expression<Func<TIn, bool>> expression, Func<TOut> setter)
    {
        if (collection is null) return default;

        var itemFromCache = collection.FirstOrDefault(expression);

        if (itemFromCache is not null)
            return itemFromCache.Adapt<TOut>();

        var itemFromSetter = setter();

        if (itemFromSetter is null)
            return default;

        collection.Insert(itemFromSetter.Adapt<TIn>());
        return itemFromSetter;

    }

    public static async Task<List<TOut>> GetOrSetAsync<TIn, TOut>(this IRedisCollection<TIn>? collection, Func<Task<List<TOut>>> setter)
    {
        if (collection is null) return [];

        if (await collection.CountAsync() > 0)
            return collection.Adapt<List<TOut>>();

        var items = await setter();

        if (items.Count == 0) return [];

        foreach (var item in items)
            await collection.InsertAsync(item.Adapt<TIn>());

        return items;

    }

    public static async Task<List<TOut>> GetOrSetAsync<TIn, TOut>(this IRedisCollection<TIn>? collection, Expression<Func<TIn, bool>> expression, Func<Task<List<TOut>>> setter) where TIn : class
    {
        if (collection is null) return [];

        var cached = await collection.Where(expression).ToListAsync();

        if (cached.Count > 0)
            return cached.Adapt<List<TOut>>();

        var items = await setter();

        if (items.Count == 0) return [];

        foreach (var item in items)
            await collection.InsertAsync(item.Adapt<TIn>());

        return items;

    }

    public static async Task<TOut?> GetOrSetAsync<TIn, TOut>(this IRedisCollection<TIn>? collection, Expression<Func<TIn, bool>> expression, Func<Task<TOut>> setter)
    {
        if (collection is null) return default;

        var itemFromCache = await collection.FirstOrDefaultAsync(expression);

        if (itemFromCache is not null)
            return itemFromCache.Adapt<TOut>();

        var itemFromSetter = await setter();

        if (itemFromSetter is null)
            return default;

        await collection.InsertAsync(itemFromSetter.Adapt<TIn>());
        return itemFromSetter;

    }

    public static string? Insert<TIn, TParam>(this IRedisCollection<TIn>? collection, TParam? param)
    {
        if (collection is not null && param is not null)
            return collection.Insert(param.Adapt<TIn>());

        return null;
    }

    public static async Task<string?> InsertAsync<TIn, TParam>(this IRedisCollection<TIn>? collection, TParam? param)
    {
        if (collection is not null && param is not null)
            return await collection.InsertAsync(param.Adapt<TIn>());

        return null;
    }

    public static void Update<TIn, TParam>(this IRedisCollection<TIn>? collection, TParam? param)
    {
        if (collection is not null && param is not null)
            collection.Update(param.Adapt<TIn>());
    }

    public static async Task UpdateAsync<TIn, TParam>(this IRedisCollection<TIn>? collection, TParam? param)
    {
        if (collection is not null && param is not null)
            await collection.UpdateAsync(param.Adapt<TIn>());
    }

    public static void Delete<TIn, TParam>(this IRedisCollection<TIn>? collection, TParam? param)
    {
        if (collection is not null && param is not null)
            collection.Delete(param.Adapt<TIn>());
    }

    public static async Task DeleteAsync<TIn, TParam>(this IRedisCollection<TIn>? collection, TParam? param)
    {
        if (collection is not null && param is not null)
            await collection.DeleteAsync(param.Adapt<TIn>());
    }
}
