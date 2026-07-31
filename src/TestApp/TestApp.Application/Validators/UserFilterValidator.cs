using FluentValidation;
using TestApp.Application.Filters;
using TestApp.Application.Validators.Messages;

namespace TestApp.Application.Validators;

public class UserFilterValidator : AbstractValidator<UserFilter>
{
    private const int MinNameLength = 2;
    private const int MaxNameLength = 30;
    private const string NamePattern = "^[a-zA-Zа-яА-Я- ]+$";
    private const int MinPageSize = 1;
    private const int MaxPageSize = 100;
    private const int MinPageNumber = 1;

    public UserFilterValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(MinPageNumber)
            .WithMessage(ValidationMessages.GreaterThanOrEqual("Page", MinPageNumber));

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(MinPageSize)
            .WithMessage(ValidationMessages.GreaterThanOrEqual("Page size", MinPageSize))
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage(ValidationMessages.LessThanOrEqual("Page size", MaxPageSize));

        When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
        {
            RuleFor(x => x.FirstName!)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(ValidationMessages.Range("First name", MinNameLength, MaxNameLength))
                .Matches(NamePattern)
                .WithMessage(ValidationMessages.MatchesRegex("First name"));
        });

        When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
        {
            RuleFor(x => x.LastName!)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(ValidationMessages.Range("Last name", MinNameLength, MaxNameLength))
                .Matches(NamePattern)
                .WithMessage(ValidationMessages.MatchesRegex("Last name"));
        });

        When(x => !string.IsNullOrWhiteSpace(x.MiddleName), () =>
        {
            RuleFor(x => x.MiddleName!)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(ValidationMessages.Range("Middle name", MinNameLength, MaxNameLength))
                .Matches(NamePattern)
                .WithMessage(ValidationMessages.MatchesRegex("Middle name"));
        });

        When(x => !string.IsNullOrWhiteSpace(x.Country), () =>
        {
            RuleFor(x => x.Country!)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(ValidationMessages.Range("Country", MinNameLength, MaxNameLength))
                .Matches(NamePattern)
                .WithMessage(ValidationMessages.MatchesRegex("Country"));
        });

        When(x => !string.IsNullOrWhiteSpace(x.City), () =>
        {
            RuleFor(x => x.City!)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(ValidationMessages.Range("City", MinNameLength, MaxNameLength))
                .Matches(NamePattern)
                .WithMessage(ValidationMessages.MatchesRegex("City"));
        });

        When(x => x.DataCollectedDateFrom.HasValue, () =>
        {
            RuleFor(x => x.DataCollectedDateFrom!.Value)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(ValidationMessages.InvalidDate("Data collected date from"));
        });

        When(x => x.DataCollectedDateTo.HasValue, () =>
        {
            RuleFor(x => x.DataCollectedDateTo!.Value)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(ValidationMessages.InvalidDate("Data collected date to"));
        });

        When(x => x.DataCollectedDateFrom.HasValue && x.DataCollectedDateTo.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.DataCollectedDateFrom <= x.DataCollectedDateTo)
                .WithMessage(ValidationMessages.DateRangeInvalid("Data collected date from", "Data collected date to"));
        });
    }
}
