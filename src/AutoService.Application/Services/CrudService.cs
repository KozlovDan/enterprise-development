using System.ComponentModel.DataAnnotations;
using AutoService.Application.Interfaces;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Application.Services;

/// <summary>Общий порядок CRUD: проверка, работа с хранилищем, преобразование в DTO.</summary>
/// <typeparam name="TEntity">Сущность.</typeparam>
/// <typeparam name="TRequest">Входной контракт.</typeparam>
/// <typeparam name="TDto">Выходной контракт.</typeparam>
/// <param name="session">Сеанс доступа к связанным данным.</param>
/// <param name="repository">Коллекция сущностей.</param>
public abstract class CrudService<TEntity, TRequest, TDto>(
    IAutoServiceSession session,
    IRepository<TEntity> repository) : ICrudService<TRequest, TDto>
    where TEntity : class, IEntity
    where TRequest : class
{
    /// <summary>Согласованный доступ к связанным справочникам.</summary>
    protected IAutoServiceSession Session { get; } = session;

    /// <inheritdoc />
    public Task<IReadOnlyList<TDto>> GetAllAsync() => Session.ExecuteReadAsync<IReadOnlyList<TDto>>(async () =>
        [.. (await repository.GetAllAsync()).Select(ToDto)]);

    /// <inheritdoc />
    public Task<TDto> GetAsync(int id) => Session.ExecuteReadAsync(async () => ToDto(await RequireAsync(repository, id)));

    /// <inheritdoc />
    public Task<TDto> CreateAsync(TRequest request) => Session.ExecuteWriteAsync(async () =>
    {
        Validate(request);
        var entity = await BuildAsync(request, 0);
        return ToDto(await repository.AddAsync(entity));
    });

    /// <inheritdoc />
    public Task UpdateAsync(int id, TRequest request) => Session.ExecuteWriteAsync(async () =>
    {
        _ = await RequireAsync(repository, id);
        Validate(request);
        var values = await BuildAsync(request, id);
        await repository.UpdateAsync(id, entity => Copy(values, entity));
    });

    /// <inheritdoc />
    public Task DeleteAsync(int id) => Session.ExecuteWriteAsync(async () =>
    {
        var entity = await RequireAsync(repository, id);
        await CheckDeleteAsync(entity);
        await repository.DeleteAsync(id);
    });

    /// <summary>Проверяет связи и подготавливает все значения до изменения хранилища.</summary>
    protected abstract Task<TEntity> BuildAsync(TRequest request, int id);
    /// <summary>Применяет проверенные значения к существующему объекту.</summary>
    protected abstract void Copy(TEntity source, TEntity target);
    /// <summary>Преобразует сущность в контракт.</summary>
    protected abstract TDto ToDto(TEntity entity);
    /// <summary>Проверяет, что удаляемая сущность не используется.</summary>
    protected abstract Task CheckDeleteAsync(TEntity entity);

    /// <summary>Находит сущность по Id или выбрасывает EntityNotFoundException.</summary>
    protected static async Task<T> RequireAsync<T>(IRepository<T> source, int id) where T : class, IEntity =>
        await source.GetByIdAsync(id) ?? throw new EntityNotFoundException($"Запись с идентификатором {id} не найдена.");

    /// <summary>Находит ссылку из входного контракта; отсутствующая ссылка означает ошибку ввода.</summary>
    protected static async Task<T> ReferenceAsync<T>(IRepository<T> source, int id, string name) where T : class, IEntity =>
        await source.GetByIdAsync(id) ?? throw new DomainValidationException($"{name} с идентификатором {id} не существует.");

    private static void Validate(TRequest request)
    {
        List<ValidationResult> results = [];
        if (!Validator.TryValidateObject(request, new(request), results, validateAllProperties: true))
        {
            throw new DomainValidationException(string.Join(" ", results.Select(result => result.ErrorMessage)));
        }
    }
}
