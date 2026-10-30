using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<InvoiceDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceDto>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<InvoiceDto>();
            result.Value = await _dbContext.Invoices
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new InvoiceDto
                {
                    Id = x.Id,
                    RegistrationId = x.RegistrationId,
                    InvoiceNumber = x.InvoiceNumber,
                    Amount = x.Amount,
                    UserFullName = x.Registration.User.FirstName + " " + x.Registration.User.LastName,
                    EventName = x.Registration.Event.Name,
                    PaidAmount = x.Payments.Where(p => p.Status == PaymentStatus.Paid).Sum(p => p.Amount),
                    Payments = x.Payments.OrderBy(p => p.PaymentDate).Select(p => new PaymentSummaryDto
                    {
                        Id = p.Id,
                        Amount = p.Amount,
                        PaymentDate = p.PaymentDate,
                        Status = p.Status
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
