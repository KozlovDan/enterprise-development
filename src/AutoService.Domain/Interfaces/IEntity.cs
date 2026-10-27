namespace AutoService.Domain.Interfaces;

/// <summary>Объект с идентификатором, назначаемым хранилищем.</summary>
public interface IEntity
{
    /// <summary>Идентификатор записи.</summary>
    public int Id { get; set; }
}
