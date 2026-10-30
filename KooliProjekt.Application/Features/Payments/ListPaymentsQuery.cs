using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    public class ListPaymentsQuery : IRequest<OperationResult<PagedResult<PaymentListItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
