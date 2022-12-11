namespace EF.Core.Extensions;

public static class QueryableExtensions
{
    #region Where
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> expression)
        => condition ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> expression)
        => condition ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, bool>> expression)
        => condition() ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, int, bool>> expression)
        => condition() ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<bool>> condition, Expression<Func<T, bool>> expression)
        => condition.Compile()() ? source.Where(expression) : source;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<bool>> condition, Expression<Func<T, int, bool>> expression)
        => condition.Compile()() ? source.Where(expression) : source;
    #endregion

    #region Include
    public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class
        => condition ? source.Include(navigationPropertyPath) : source;

    public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(this IQueryable<TEntity> source, Func<bool> condition, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class
        => condition() ? source.Include(navigationPropertyPath) : source;

    public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<bool>> condition, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class
        => condition.Compile()() ? source.Include(navigationPropertyPath) : source;
    #endregion

    #region AsNoTrackingIf
    public static IQueryable<TEntity> AsNoTrackingIf<TEntity, TProperty>(this IQueryable<TEntity> source, bool condition)
        where TEntity : class
        => condition ? source.AsNoTracking() : source;

    public static IQueryable<TEntity> AsNoTrackingIf<TEntity, TProperty>(this IQueryable<TEntity> source, Func<bool> condition)
        where TEntity : class
        => condition() ? source.AsNoTracking() : source;

    public static IQueryable<TEntity> AsNoTrackingIf<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<bool>> condition)
        where TEntity : class
        => condition.Compile()() ? source.AsNoTracking() : source;
    #endregion

}
