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
    public IReadOnlyList<TDto> GetAll() => Session.Execute<IReadOnlyList<TDto>>(() =>
        [.. repository.GetAll().Select(ToDto)]);

    /// <inheritdoc />
    public TDto Get(int id) => Session.Execute(() => ToDto(Require(repository, id)));

    /// <inheritdoc />
    public TDto Create(TRequest request) => Session.Execute(() =>
    {
        Validate(request);
        var entity = Build(request, 0);
        return ToDto(repository.Add(entity));
    });

    /// <inheritdoc />
    public void Update(int id, TRequest request) => Session.Execute(() =>
    {
        _ = Require(repository, id);
        Validate(request);
        var values = Build(request, id);
        repository.Update(id, entity => Copy(values, entity));
    });

    /// <inheritdoc />
    public void Delete(int id) => Session.Execute(() =>
    {
        var entity = Require(repository, id);
        CheckDelete(entity);
        repository.Delete(id);
    });

    /// <summary>Проверяет связи и подготавливает все значения до изменения хранилища.</summary>
    protected abstract TEntity Build(TRequest request, int id);
    /// <summary>Применяет проверенные значения к существующему объекту.</summary>
    protected abstract void Copy(TEntity source, TEntity target);
    /// <summary>Преобразует сущность в контракт.</summary>
    protected abstract TDto ToDto(TEntity entity);
    /// <summary>Проверяет, что удаляемая сущность не используется.</summary>
    protected abstract void CheckDelete(TEntity entity);

    /// <summary>Находит запрашиваемый ресурс или сообщает о его отсутствии.</summary>
    protected static T Require<T>(IRepository<T> source, int id) where T : class, IEntity =>
        source.GetById(id) ?? throw new EntityNotFoundException($"Запись с идентификатором {id} не найдена.");

    /// <summary>Находит ссылку из входного контракта; отсутствующая ссылка означает ошибку ввода.</summary>
    protected static T Reference<T>(IRepository<T> source, int id, string name) where T : class, IEntity =>
        source.GetById(id) ?? throw new DomainValidationException($"{name} с идентификатором {id} не существует.");

    private static void Validate(TRequest request)
    {
        List<ValidationResult> results = [];
        if (!Validator.TryValidateObject(request, new(request), results, validateAllProperties: true))
        {
            throw new DomainValidationException(string.Join(" ", results.Select(result => result.ErrorMessage)));
        }
    }
}
