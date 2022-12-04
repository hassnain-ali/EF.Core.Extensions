using System.Linq.Expressions;

namespace EF.Core.Extensions;

public static class CoreExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> expression) => condition ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> expression) => condition ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, bool>> expression) => condition() ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, int, bool>> expression) => condition() ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<bool>> condition, Expression<Func<T, bool>> expression) => condition.Compile()() ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<bool>> condition, Expression<Func<T, int, bool>> expression) => condition.Compile()() ? source.Where(expression) : source;
}
