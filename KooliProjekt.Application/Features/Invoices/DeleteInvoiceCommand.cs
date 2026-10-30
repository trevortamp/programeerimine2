using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class DeleteInvoiceCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
