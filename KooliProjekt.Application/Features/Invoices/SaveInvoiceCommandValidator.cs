using FluentValidation;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandValidator : AbstractValidator<SaveInvoiceCommand>
    {
        public SaveInvoiceCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.RegistrationId).GreaterThan(0);
            RuleFor(x => x.InvoiceNumber).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        }
    }
}
