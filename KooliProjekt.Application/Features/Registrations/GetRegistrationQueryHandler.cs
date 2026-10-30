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
    public class GetRegistrationQueryHandler : IRequestHandler<GetRegistrationQuery, OperationResult<RegistrationDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetRegistrationQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<RegistrationDto>> Handle(GetRegistrationQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<RegistrationDto>();
            result.Value = await _dbContext.Registrations
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new RegistrationDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserFullName = x.User.FirstName + " " + x.User.LastName,
                    EventId = x.EventId,
                    EventName = x.Event.Name,
                    EventStartDateTime = x.Event.StartDateTime,
                    EventPrice = x.Event.Price,
                    RegistrationDate = x.RegistrationDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
