using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Extensions;

public interface IRepository<TContext, T> where T : BaseEntity, new()
    where TContext : DbContext
{
    Task<T?> Get(Guid id, CancellationToken token = default);
    Task<IList<T>> List(CancellationToken token = default);
    Task<IList<T>> List(Expression<Func<T, bool>> expression, CancellationToken token = default);
    Task<(T, int)> Insert(T entity, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> InsertRange(IEnumerable<T> entity, CancellationToken token = default);
    Task<(T, int)> Update(T entity, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> UpdateRange(IEnumerable<T> entity, CancellationToken token = default);
    Task<(T, int)> Delete(T entity, CancellationToken token = default);
    Task<(T?, int)> Delete(Guid id, CancellationToken token = default);
    Task<(T?, int)> Delete(Expression<Func<T, bool>> expression, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<T> entity, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<Guid> id, CancellationToken token = default);
    Task<(IEnumerable<T>, int)> DeleteRange(Expression<Func<T, bool>> expression, CancellationToken token = default);
    TContext Context { get; }
    DbSet<T> Table { get; }
    IQueryable<T> Values { get; }
}
public class Repository<TContext, T> : IRepository<TContext, T>, IDisposable, IAsyncDisposable
    where T : BaseEntity, new()
    where TContext : DbContext
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
    public virtual async Task<(T?, int)> Delete(Guid id, CancellationToken token = default)
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
    public virtual async Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<Guid> id, CancellationToken token = default)
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
    public virtual async Task<T?> Get(Guid id, CancellationToken token = default) => await Table.SingleOrDefaultAsync(s => s.Id == id, token);
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
    public virtual async Task<IList<T>> List(CancellationToken token = default) => await Table.ToListAsync(token);
    public virtual async Task<IList<T>> List(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.Where(expression).ToListAsync(token);
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

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Context.Dispose();
    }
    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return Context.DisposeAsync();
    }
}