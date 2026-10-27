using System.Linq.Expressions;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>Коллекция с монотонным счётчиком идентификаторов. Используется внутри сеанса.</summary>
/// <typeparam name="T">Тип хранимой сущности.</typeparam>
public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly Dictionary<int, T> _entities;
    private int _lastId;

    /// <summary>Загружает начальные записи и устанавливает счётчик.</summary>
    public InMemoryRepository(IEnumerable<T> entities)
    {
        _entities = entities.ToDictionary(entity => entity.Id);
        _lastId = _entities.Keys.DefaultIfEmpty().Max();
    }

    /// <inheritdoc />
    public IReadOnlyList<T> GetAll() => [.. _entities.Values.OrderBy(entity => entity.Id)];

    /// <inheritdoc />
    public T? GetById(int id) => _entities.GetValueOrDefault(id);

    /// <inheritdoc />
    public IReadOnlyList<T> GetByIds(IReadOnlyCollection<int> ids) =>
        [.. ids.Distinct().Where(_entities.ContainsKey).Select(id => _entities[id])];

    /// <inheritdoc />
    public IReadOnlyList<T> Find(Expression<Func<T, bool>> predicate) =>
        [.. _entities.Values.Where(predicate.Compile()).OrderBy(entity => entity.Id)];

    /// <inheritdoc />
    public bool Exists(Expression<Func<T, bool>> predicate) => _entities.Values.Any(predicate.Compile());

    /// <inheritdoc />
    public T Add(T entity)
    {
        entity.Id = checked(++_lastId);
        _entities.Add(entity.Id, entity);
        return entity;
    }

    /// <inheritdoc />
    public void Update(int id, Action<T> update) => update(GetRequired(id));

    /// <inheritdoc />
    public void Delete(int id)
    {
        _ = GetRequired(id);
        _entities.Remove(id);
    }

    private T GetRequired(int id) => GetById(id)
        ?? throw new EntityNotFoundException($"Запись с идентификатором {id} не найдена.");
}
