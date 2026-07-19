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
        $"{fieldName} must be between {min} and {max}";

    public static string MatchesRegex(string fieldName) =>
        $"{fieldName} must be written in letters";

    public static string InvalidDate(string fieldName) =>
        $"{fieldName} must be written in letters";
}
