using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, OperationResult<PaymentDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPaymentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PaymentDto>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PaymentDto>();
            result.Value = await _dbContext.Payments
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new PaymentDto
                {
                    Id = x.Id,
                    InvoiceId = x.InvoiceId,
                    InvoiceNumber = x.Invoice.InvoiceNumber,
                    Amount = x.Amount,
                    PaymentDate = x.PaymentDate,
                    Status = x.Status
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
