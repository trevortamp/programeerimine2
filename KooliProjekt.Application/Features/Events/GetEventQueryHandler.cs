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
    public class GetEventQueryHandler : IRequestHandler<GetEventQuery, OperationResult<EventDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEventQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<EventDto>> Handle(GetEventQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<EventDto>();
            result.Value = await _dbContext.Events
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new EventDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    Description = x.Description,
                    Price = x.Price
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
