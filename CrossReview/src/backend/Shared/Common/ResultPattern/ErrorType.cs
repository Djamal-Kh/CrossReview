namespace Shared.Common.ResultPattern;

public enum ErrorType
{
    /// <summary>
    /// Ошибка валидации.
    /// </summary>
    VALIDATION,

    /// <summary>
    /// Ошибка ничего не найдено.
    /// </summary>
    NOT_FOUND,

    /// <summary>
    /// Ошибка сервера.
    /// </summary>
    FAILURE,

    /// <summary>
    /// Ошибка конфликт.
    /// </summary>
    CONFLICT,
}