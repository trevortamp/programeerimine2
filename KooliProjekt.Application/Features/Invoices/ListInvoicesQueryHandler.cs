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
    public class ListInvoicesQueryHandler : IRequestHandler<ListInvoicesQuery, OperationResult<PagedResult<InvoiceListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListInvoicesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceListItemDto>>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<InvoiceListItemDto>>();
            result.Value = await _dbContext.Invoices
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(x => new InvoiceListItemDto
                {
                    Id = x.Id,
                    InvoiceNumber = x.InvoiceNumber,
                    Amount = x.Amount,
                    UserFullName = x.Registration.User.FirstName + " " + x.Registration.User.LastName,
                    EventName = x.Registration.Event.Name
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
