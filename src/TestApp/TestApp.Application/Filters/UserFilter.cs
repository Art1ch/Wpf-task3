namespace TestApp.Application.Filters;

public sealed class UserFilter
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly? DataCollectedDateFrom { get; set; }
    public DateOnly? DataCollectedDateTo { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}