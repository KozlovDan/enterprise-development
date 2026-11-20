namespace AutoService.Application.Interfaces;

/// <summary>Общие операции создания, чтения, обновления и удаления с преобразованием данных в DTO.</summary>
/// <typeparam name="TRequest">Данные записи.</typeparam>
/// <typeparam name="TDto">Представление результата.</typeparam>
public interface ICrudService<TRequest, TDto>
{
    /// <summary>Возвращает представления всех объектов в виде <typeparamref name="TDto"/>.</summary>
    public Task<IReadOnlyList<TDto>> GetAllAsync();
    /// <summary>Находит объект по Id и возвращает его представление <typeparamref name="TDto"/>.</summary>
    public Task<TDto> GetAsync(int id);
    /// <summary>Создаёт объект по данным <typeparamref name="TRequest"/> и возвращает его представление <typeparamref name="TDto"/>.</summary>
    public Task<TDto> CreateAsync(TRequest request);
    /// <summary>Обновляет объект с указанным Id по данным <typeparamref name="TRequest"/>.</summary>
    public Task UpdateAsync(int id, TRequest request);
    /// <summary>Удаляет объект с указанным Id после проверки его использования связанными данными.</summary>
    public Task DeleteAsync(int id);
}
