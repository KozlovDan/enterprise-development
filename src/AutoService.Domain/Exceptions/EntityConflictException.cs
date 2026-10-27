namespace AutoService.Domain.Exceptions;

/// <summary>Изменение нарушает существующие связи или уникальность.</summary>
/// <param name="message">Понятное пользователю описание ошибки.</param>
public class EntityConflictException(string message) : Exception(message);
