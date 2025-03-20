using Humanizer;
using Restaurant.Shared.Common;

namespace Restaurant.API.Controllers.Helpers;

public static class TransformQueryIntoObject<T> where T : IQueryObject, new()
{
    public static T Transform(IQueryCollection queryCollection)
    {
        var item = new T();

        foreach (var (k, v) in queryCollection)
            item.SetQueryValue(k.Pascalize(), v.ToString());

        return item;
    }
}