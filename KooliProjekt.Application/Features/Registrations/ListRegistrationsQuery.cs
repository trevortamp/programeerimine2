using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Registrations
{
    public class ListRegistrationsQuery : IRequest<OperationResult<PagedResult<RegistrationListItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
