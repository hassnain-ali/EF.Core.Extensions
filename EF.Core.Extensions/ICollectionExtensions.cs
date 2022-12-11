namespace EF.Core.Extensions;

public static class ICollectionExtensions
{
    public static bool AddIf<T>(this ICollection<T> values, Func<T, bool> func, T value)
    {
        if (func(value))
        {
            values.Add(value);
            return true;
        }

        return false;
    }
    public static bool AddOrIgnore<T>(this ICollection<T> @this, T value)
    {
        if (!@this.Contains(value))
        {
            @this.Add(value);
            return true;
        }

        return false;
    }
    public static void AddRangeIfNotContains<T>(this ICollection<T> @this, params T[] values)
    {
        foreach (T value in values)
            if (!@this.Contains(value))
                @this.Add(value);
    }
    public static void AddRangeIf<T>(this ICollection<T> @this, Func<T, bool> predicate, params T[] values)
    {
        foreach (T value in values)
            if (predicate(value))
                @this.Add(value);
    }
    public static void AddRange<T>(this ICollection<T> @this, params T[] values)
    {
        foreach (T value in values)
            @this.Add(value);
    }

}
