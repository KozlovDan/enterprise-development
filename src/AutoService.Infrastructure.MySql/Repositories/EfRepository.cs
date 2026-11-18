using System.Linq.Expressions;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.MySql.Repositories;

/// <summary>Операции EF Core. Фильтрация выполняется в MySQL.</summary>
/// <typeparam name="T">Сущность справочника.</typeparam>
/// <param name="context">Контекст текущего запроса.</param>
public class EfRepository<T>(AutoServiceDbContext context) : IRepository<T> where T : class, IEntity
{
    /// <summary>Контекст текущего запроса.</summary>
    protected AutoServiceDbContext Context { get; } = context;
    /// <summary>Запрос с необходимыми для DTO навигациями.</summary>
    protected virtual IQueryable<T> Query => Context.Set<T>();

    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> GetAllAsync() => await Query.OrderBy(entity => entity.Id).ToListAsync();
    /// <inheritdoc />
    public Task<T?> GetByIdAsync(int id)
    {
        var tracked = Context.Set<T>().Local.FirstOrDefault(entity => entity.Id == id);
        return tracked is null
            ? Query.SingleOrDefaultAsync(entity => entity.Id == id)
            : Task.FromResult<T?>(tracked);
    }
    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> GetByIdsAsync(IReadOnlyCollection<int> ids) =>
        await Query.Where(entity => ids.Contains(entity.Id)).OrderBy(entity => entity.Id).ToListAsync();
    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await Query.Where(predicate).OrderBy(entity => entity.Id).ToListAsync();
    /// <inheritdoc />
    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => Context.Set<T>().AnyAsync(predicate);

    /// <inheritdoc />
    public async Task<T> AddAsync(T entity)
    {
        Context.Set<T>().Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, Action<T> update)
    {
        var entity = await Context.Set<T>().FindAsync(id)
            ?? throw new EntityNotFoundException($"Запись с идентификатором {id} не найдена.");
        update(entity);
        await Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        var entity = await Context.Set<T>().FindAsync(id)
            ?? throw new EntityNotFoundException($"Запись с идентификатором {id} не найдена.");
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
    }
}
