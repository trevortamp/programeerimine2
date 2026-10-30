using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQuery : IRequest<OperationResult<PaymentDto>>
    {
        public int Id { get; set; }
    }
}
