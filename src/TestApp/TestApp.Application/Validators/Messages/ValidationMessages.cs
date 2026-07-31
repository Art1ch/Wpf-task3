namespace TestApp.Application.Validators.Messages;

public static class ValidationMessages
{
    public static string Required(string fieldName) =>
        $"{fieldName} is required";

    public static string NotNullBatch(string fieldName) =>
        $"{fieldName} can't be null";

    public static string MaxLength(string fieldName, int max) =>
        $"{fieldName} cannot exceed {max} characters";

    public static string Range(string fieldName, int min, int max) =>
        $"{fieldName} must be between {min} and {max} characters";

    public static string MatchesRegex(string fieldName) =>
        $"{fieldName} must contain only letters, hyphens, and spaces";

    public static string InvalidDate(string fieldName) =>
        $"{fieldName} cannot be in the future";

    public static string GreaterThanOrEqual(string fieldName, int value) =>
        $"{fieldName} must be at least {value}";

    public static string LessThanOrEqual(string fieldName, int value) =>
        $"{fieldName} cannot exceed {value}";

    public static string DateRangeInvalid(string fromField, string toField) =>
        $"'{fromField}' must be before or equal to '{toField}'";
}
