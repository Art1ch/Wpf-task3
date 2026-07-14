using FluentValidation;
using TestApp.Application.Validators.Messages;
using TestApp.Core.Models;

namespace TestApp.Application.Validators;

public sealed class UserImportModelBatchValidator : AbstractValidator<IEnumerable<UserImportModel>>
{
    public UserImportModelBatchValidator()
    {
        var _itemValidator = new UserImportModelValidator();

        RuleFor(x => x)
            .NotNull().WithMessage(ValidationMessages.NotNullBatch("List of users"));

        RuleForEach(x => x)
            .SetValidator(_itemValidator);
    }
}