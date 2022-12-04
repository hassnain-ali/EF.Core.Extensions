using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Extensions;

#if NET6_0_OR_GREATER

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
public sealed class Repository<TContext, T> : IRepository<TContext, T>, IDisposable, IAsyncDisposable
    where T : BaseEntity, new()
    where TContext : DbContext
{
    public Repository(TContext context)
    {
        Context = context;
        Table = context.Set<T>();
    }
    public DbSet<T> Table { get; }

    public IQueryable<T> Values => Table.AsNoTracking();

    public TContext Context { get; }

    public async Task<(T, int)> Delete(T entity, CancellationToken token = default)
    {
        Table.Remove(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public async Task<(T?, int)> Delete(Guid id, CancellationToken token = default)
    {
        var entity = await Table.FindAsync(new object?[] { id }, cancellationToken: token);
        if (entity == null)
        {
            return (null, 0);
        }
        Table.Remove(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public async Task<(T?, int)> Delete(Expression<Func<T, bool>> expression, CancellationToken token = default)
    {
        var entity = await Table.FirstOrDefaultAsync(expression, token);
        if (entity == null)
        {
            return (null, 0);
        }
        Table.Remove(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public async Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<T> entity, CancellationToken token = default)
    {
        Table.RemoveRange(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public async Task<(IEnumerable<T>, int)> DeleteRange(IEnumerable<Guid> id, CancellationToken token = default)
    {
        var entities = Table.Where(s => id.Contains(s.Id));
        Table.RemoveRange(entities);
        return (entities, await Context.SaveChangesAsync(token));
    }
    public async Task<(IEnumerable<T>, int)> DeleteRange(Expression<Func<T, bool>> expression, CancellationToken token = default)
    {
        var entities = Table.Where(expression);
        Table.RemoveRange(entities);
        return (entities, await Context.SaveChangesAsync(token));
    }
    public async Task<T?> Get(Guid id, CancellationToken token = default) => await Table.SingleOrDefaultAsync(s => s.Id == id, token);
    public async Task<(T, int)> Insert(T entity, CancellationToken token)
    {
        await Table.AddAsync(entity, token);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public async Task<(IEnumerable<T>, int)> InsertRange(IEnumerable<T> entity, CancellationToken token = default)
    {
        await Table.AddRangeAsync(entity, token);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public async Task<IList<T>> List(CancellationToken token = default) => await Table.ToListAsync(token);
    public async Task<IList<T>> List(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.Where(expression).ToListAsync(token);
    public async Task<(T, int)> Update(T entity, CancellationToken token = default)
    {
        Table.Update(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }
    public async Task<(IEnumerable<T>, int)> UpdateRange(IEnumerable<T> entity, CancellationToken token = default)
    {
        Table.UpdateRange(entity);
        return (entity, await Context.SaveChangesAsync(token));
    }

    public void Dispose() => Context.Dispose();
    public ValueTask DisposeAsync() => Context.DisposeAsync();
}
#endif