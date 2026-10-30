using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Events
{
    public class ListEventsQueryHandler : IRequestHandler<ListEventsQuery, OperationResult<PagedResult<EventListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListEventsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<EventListItemDto>>> Handle(ListEventsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<EventListItemDto>>();
            result.Value = await _dbContext.Events
                .AsNoTracking()
                .OrderBy(x => x.StartDateTime).ThenBy(x => x.Id)
                .Select(x => new EventListItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDateTime = x.StartDateTime,
                    Price = x.Price
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
