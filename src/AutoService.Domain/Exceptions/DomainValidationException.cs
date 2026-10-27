namespace AutoService.Domain.Exceptions;

/// <summary>Данные не соответствуют правилам предметной области.</summary>
/// <param name="message">Понятное пользователю описание ошибки.</param>
public class DomainValidationException(string message) : Exception(message);
