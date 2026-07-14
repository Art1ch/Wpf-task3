using TestApp.Application.Validators.Base;
using TestApp.Core.Models;

namespace TestApp.Application.Validators;

public sealed class UserImportModelValidator : UserValidatorBase<UserImportModel>
{
    public UserImportModelValidator()
    {
        ValidateFirstName(x => x.FirstName);
        ValidateLastName(x => x.LastName);
        ValidateMiddleName(x => x.MiddleName);
        ValidateDataCollectedDate(x => x.DataCollectedDate);
    }
}
