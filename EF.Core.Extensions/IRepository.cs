namespace EF.Core.Extensions;

public interface IRepository<TContext, T, Tkey> : IDisposable, IAsyncDisposable
    where TContext : DbContext
    where T : class, IBaseEntity<Tkey>
    where Tkey : IEquatable<Tkey>
{
    Task<T?> Get(Tkey id, CancellationToken token = default);
    Task<IList<T>> List(CancellationToken token = default);
    Task<IList<T>> List(Expression<Func<T, bool>> expression, CancellationToken token = default);
    Task<(T, int)> Insert(T entity, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> InsertRange(IEnumerable<T> entity, CancellationToken token = default);
    Task<(T, int)> Update(T entity, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> UpdateRange(IEnumerable<T> entity, CancellationToken token = default);
    Task<(T, int)> Delete(T entity, CancellationToken token = default);
    Task<(T?, int)> Delete(Tkey id, CancellationToken token = default);
    Task<(T?, int)> Delete(Expression<Func<T, bool>> expression, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<T> entity, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<Tkey> id, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> DeleteRange(Expression<Func<T, bool>> expression, CancellationToken token = default);
    TContext Context { get; }
    DbSet<T> Table { get; }
    IQueryable<T> Values { get; }
}

public abstract partial class Repository<TContext, T, TKey> : IRepository<TContext, T, TKey>,
    IDisposable,
    IAsyncDisposable
    where TContext : DbContext
    where T : class, IBaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public Repository(TContext context)
    {
        Context = context;
        Table = context.Set<T>();
    }
    public virtual DbSet<T> Table { get; private set; }

    public virtual IQueryable<T> Values => Table.AsNoTracking();

    public virtual TContext Context { get; private set; }

    public virtual async Task<(T, int)> Delete(T entity, CancellationToken token = default)
    {
        _ = Table.Remove(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<(T?, int)> Delete(TKey id, CancellationToken token = default)
    {
        T? entity = await Table.FindAsync(new object?[] { id }, cancellationToken: token);
        if (entity == null)
        {
            return (null, 0);
        }
        _ = Table.Remove(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<(T?, int)> Delete(Expression<Func<T, bool>> expression, CancellationToken token = default)
    {
        T? entity = await Table.FirstOrDefaultAsync(expression, token);
        if (entity == null)
        {
            return (null, 0);
        }
        _ = Table.Remove(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<T> entity, CancellationToken token = default)
    {
        Table.RemoveRange(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<TKey> id, CancellationToken token = default)
    {
        IQueryable<T> entities = Table.Where(s => id.Contains(s.Id));
        Table.RemoveRange(entities);
        return (entities, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<(IEnumerable<T>, int)> DeleteRange(Expression<Func<T, bool>> expression, CancellationToken token = default)
    {
        IQueryable<T> entities = Table.Where(expression);
        Table.RemoveRange(entities);
        return (entities, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<T?> Get(TKey id, CancellationToken token = default) => await Table.AsNoTracking().SingleOrDefaultAsync(s => s.Id.Equals(id), token);
    public virtual async Task<(T, int)> Insert(T entity, CancellationToken token)
    {
        _ = await Table.AddAsync(entity, token);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<(IEnumerable<T>, int)> InsertRange(IEnumerable<T> entity, CancellationToken token = default)
    {
        await Table.AddRangeAsync(entity, token);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<IList<T>> List(CancellationToken token = default) => await Values.ToListAsync(token);
    public virtual async Task<IList<T>> List(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Values.Where(expression).ToListAsync(token);
    public virtual async Task<(T, int)> Update(T entity, CancellationToken token = default)
    {
        _ = Table.Update(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public virtual async Task<(IEnumerable<T>, int)> UpdateRange(IEnumerable<T> entity, CancellationToken token = default)
    {
        Table.UpdateRange(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Context.Dispose();
    }
    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return Context.DisposeAsync();
    }
}