namespace AutoService.Domain.Exceptions;

/// <summary>Запрошенная запись не найдена.</summary>
/// <param name="message">Понятное пользователю описание ошибки.</param>
public class EntityNotFoundException(string message) : Exception(message);
