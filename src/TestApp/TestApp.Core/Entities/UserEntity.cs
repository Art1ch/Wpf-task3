using TestApp.Core.Entities.Base;

namespace TestApp.Core.Entities;

public sealed class UserEntity : EntityBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateOnly DataCollectedDate { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
}
