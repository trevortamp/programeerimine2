using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Registrations
{
    public class ListRegistrationsQueryHandler : IRequestHandler<ListRegistrationsQuery, OperationResult<PagedResult<RegistrationListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListRegistrationsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<RegistrationListItemDto>>> Handle(ListRegistrationsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<RegistrationListItemDto>>();
            result.Value = await _dbContext.Registrations
                .AsNoTracking()
                .OrderByDescending(x => x.RegistrationDate).ThenByDescending(x => x.Id)
                .Select(x => new RegistrationListItemDto
                {
                    Id = x.Id,
                    UserFullName = x.User.FirstName + " " + x.User.LastName,
                    EventName = x.Event.Name,
                    RegistrationDate = x.RegistrationDate
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
