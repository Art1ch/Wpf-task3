namespace TestApp.Core.Models;

public sealed class UserExportModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateOnly DataCollectedDate { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
}
