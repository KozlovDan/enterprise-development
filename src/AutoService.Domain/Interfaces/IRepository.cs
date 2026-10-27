using System.Linq.Expressions;

namespace AutoService.Domain.Interfaces;

/// <summary>Операции над коллекцией сущностей в рамках сеанса хранилища.</summary>
/// <typeparam name="T">Тип сущности.</typeparam>
public interface IRepository<T> where T : class, IEntity
{
    /// <summary>Возвращает записи в порядке идентификаторов.</summary>
    public IReadOnlyList<T> GetAll();

    /// <summary>Возвращает запись или null.</summary>
    public T? GetById(int id);

    /// <summary>Отбирает записи по набору идентификаторов.</summary>
    public IReadOnlyList<T> GetByIds(IReadOnlyCollection<int> ids);

    /// <summary>Отбирает записи на стороне хранилища.</summary>
    public IReadOnlyList<T> Find(Expression<Func<T, bool>> predicate);

    /// <summary>Проверяет наличие подходящей записи без создания списка.</summary>
    public bool Exists(Expression<Func<T, bool>> predicate);

    /// <summary>Добавляет сущность и назначает новый идентификатор.</summary>
    public T Add(T entity);

    /// <summary>Обновляет существующий объект, сохраняя ссылки на него.</summary>
    public void Update(int id, Action<T> update);

    /// <summary>Удаляет существующую запись.</summary>
    public void Delete(int id);
}
