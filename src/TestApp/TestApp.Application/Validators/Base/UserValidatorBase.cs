using FluentValidation;
using System.Linq.Expressions;
using TestApp.Application.Validators.Messages;

namespace TestApp.Application.Validators.Base;

public abstract class UserValidatorBase<T> : AbstractValidator<T>
{
    private const int MinNameLength = 2;
    private const int MaxNameLength = 30;

    protected void ValidateFirstName(Expression<Func<T, string>> expression)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage(ValidationMessages.Required("First name"))
            .Length(MinNameLength, MaxNameLength).WithMessage(ValidationMessages.Range("First name", MinNameLength, MaxNameLength))
            .Matches("^[a-zA-Zа-яА-Я- ]+$").WithMessage(ValidationMessages.MatchesRegex("First name"));
    }

    protected void ValidateLastName(Expression<Func<T, string>> expression)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage(ValidationMessages.Required("Last name"))
            .Length(MinNameLength, MaxNameLength).WithMessage(ValidationMessages.Range("Last name", MinNameLength, MaxNameLength))
            .Matches("^[a-zA-Zа-яА-Я- ]+$").WithMessage(ValidationMessages.MatchesRegex("Last name"));
    }

    protected void ValidateMiddleName(Expression<Func<T, string>> expression)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage(ValidationMessages.Required("Middle name"))
            .Length(MinNameLength, MaxNameLength).WithMessage(ValidationMessages.Range("Middle name", MinNameLength, MaxNameLength))
            .Matches("^[a-zA-Zа-яА-Я- ]+$").WithMessage(ValidationMessages.MatchesRegex("Middle name"));
    }

    protected void ValidateCountryName(Expression<Func<T, string>> expression)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage(ValidationMessages.Required("Country name"))
            .Length(MinNameLength, MaxNameLength).WithMessage(ValidationMessages.Range("Country name", MinNameLength, MaxNameLength))
            .Matches("^[a-zA-Zа-яА-Я- ]+$").WithMessage(ValidationMessages.MatchesRegex("Country name"));
    }

    protected void ValidateCityName(Expression<Func<T, string>> expression)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage(ValidationMessages.Required("City name"))
            .Length(MinNameLength, MaxNameLength).WithMessage(ValidationMessages.Range("City name", MinNameLength, MaxNameLength))
            .Matches("^[a-zA-Zа-яА-Я- ]+$").WithMessage(ValidationMessages.MatchesRegex("City name"));
    }

    protected void ValidateDataCollectedDate(Expression<Func<T, DateOnly>> expression)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage(ValidationMessages.Required("Data collected date"))
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ValidationMessages.InvalidDate("Data collected date"));
    }
}
