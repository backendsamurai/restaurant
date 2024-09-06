using System.Linq.Expressions;
using Mapster;
using Redis.OM.Searching;

namespace Restaurant.API.Extensions;

public static class RedisExtensions
{
    public static List<TOut> GetOrSet<TIn, TOut>(this IRedisCollection<TIn>? collection, Func<List<TOut>> setter)
    {
        if (collection is not null && collection.Count() > 0)
            return collection.Adapt<List<TOut>>();

        var items = setter();

        if (items is not null && items.Count > 0)
        {
            foreach (var item in items)
                collection?.Insert(item.Adapt<TIn>());

            return items;
        }

        return [];
    }

    public static TOut? GetOrSet<TIn, TOut>(this IRedisCollection<TIn>? collection, Expression<Func<TIn, bool>> expression, Func<TOut> setter)
    {
        if (collection is not null)
        {
            var itemFromCache = collection.FirstOrDefault(expression);

            if (itemFromCache is not null)
                return itemFromCache.Adapt<TOut>();

            var itemFromSetter = setter();

            if (itemFromSetter is null)
                return default;

            collection.Insert(itemFromSetter.Adapt<TIn>());
            return itemFromSetter;
        }

        return default;
    }

    public static async Task<List<TOut>> GetOrSetAsync<TIn, TOut>(this IRedisCollection<TIn>? collection, Func<Task<List<TOut>>> setter)
    {
        if (collection is not null)
        {
            if (await collection.CountAsync() > 0)
                return collection.Adapt<List<TOut>>();

            var items = await setter();

            if (items is not null && items.Count > 0)
            {
                foreach (var item in items)
                    await collection.InsertAsync(item.Adapt<TIn>());

                return items;
            }
        }

        return [];
    }

    public static async Task<TOut?> GetOrSetAsync<TIn, TOut>(this IRedisCollection<TIn>? collection, Expression<Func<TIn, bool>> expression, Func<Task<TOut>> setter)
    {
        if (collection is not null)
        {
            var itemFromCache = await collection.FirstOrDefaultAsync(expression);

            if (itemFromCache is not null)
                return itemFromCache.Adapt<TOut>();

            var itemFromSetter = await setter();

            if (itemFromSetter is null)
                return default;

            await collection.InsertAsync(itemFromSetter.Adapt<TIn>());
            return itemFromSetter;
        }

        return default;
    }
}
