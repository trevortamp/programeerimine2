using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Events
{
    public class GetEventQuery : IRequest<OperationResult<EventDto>>
    {
        public int Id { get; set; }
    }
}
