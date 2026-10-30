using FluentValidation;

namespace KooliProjekt.Application.Features.Events
{
    public class SaveEventCommandValidator : AbstractValidator<SaveEventCommand>
    {
        public SaveEventCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.StartDateTime).NotEmpty();
            RuleFor(x => x.EndDateTime).NotEmpty()
                .GreaterThan(x => x.StartDateTime)
                .WithMessage("End date must be later than start date");
            RuleFor(x => x.Description).MaximumLength(2000);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}
