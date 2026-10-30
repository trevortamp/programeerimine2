using FluentValidation;

namespace KooliProjekt.Application.Features.Registrations
{
    public class SaveRegistrationCommandValidator : AbstractValidator<SaveRegistrationCommand>
    {
        public SaveRegistrationCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.EventId).GreaterThan(0);
        }
    }
}
