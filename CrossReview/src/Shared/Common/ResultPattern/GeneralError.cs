namespace Shared.Common.ResultPattern;

public static class GeneralErrors
{
    public static Error ValueIsInvalid(string? name = null)
    {
        string label = name ?? ". Значение";
        return Error.Validation("value.is.invalid", $"{label} недействительно");
    }

    public static Error NotFound(int? id = null)
    {
        string forId = id == null ? string.Empty : $" по Id '{id}'";
        return Error.NotFound("record.not.found", $"запись не найдена {forId} ");
    }
    
    public static Error NotFound(Guid? id = null)
    {
        string forId = id == null ? string.Empty : $" по Id '{id}'";
        return Error.NotFound("record.not.found", $"запись не найдена {forId} ");
    }

    public static Error CollectionEmpty()
    {
        return Error.NotFound("record.not.found", "ни одна запись не найдена");
    }

    public static Error ValueIsRequired(string? name = null)
    {
        string label = name == null ? string.Empty : " " + name + " ";
        return Error.Validation("length.is.invalid", $"Поле {label} обязательно");
    }

    public static Error Failure(string? message = null)
    {
        return Error.Failure("server.failure", message ?? "Серверная ошибка");
    }

    public static Error ValueAlreadyExists(string? name = null)
    {
        string label = name ?? "значение";
        return Error.Validation("value.already.exists", $"{label} уже существует");
    }

    public static Error DuplicateValues(string? name = null)
    {
        string label = name ?? "коллекции";
        return Error.Validation("duplicate.values", $"В {label} значения дублируются");
    }

    public static Error ValueTooLong(int lenght, string? name = null)
    {
        string label = name ?? "значение";
        return Error.Validation("value.is.too.long",
            $"{label} слишком длинное. Максимальное количество символов: {lenght}");
    }
    
    public static Error ValueTooShort(int lenght, string? name = null)
    {
        string label = name ?? "значение";
        return Error.Validation("value.is.too.short",
            $"{label} слишком длинное. Максимальное количество символов: {lenght}");
    }

    public static Error NotInRangeDateTime(DateTime start, DateTime end)
    {
        return Error.Failure("not.in.range.datetime", $"Период активности: от {start} до {end}");
    }
}