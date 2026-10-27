namespace AutoService.Application.Interfaces;

/// <summary>Операции над входными и выходными контрактами ресурса.</summary>
/// <typeparam name="TRequest">Данные записи.</typeparam>
/// <typeparam name="TDto">Представление результата.</typeparam>
public interface ICrudService<TRequest, TDto>
{
    /// <summary>Возвращает все записи.</summary>
    public IReadOnlyList<TDto> GetAll();
    /// <summary>Возвращает запись по идентификатору.</summary>
    public TDto Get(int id);
    /// <summary>Создаёт запись.</summary>
    public TDto Create(TRequest request);
    /// <summary>Полностью обновляет существующую запись.</summary>
    public void Update(int id, TRequest request);
    /// <summary>Удаляет запись, если она не используется.</summary>
    public void Delete(int id);
}
