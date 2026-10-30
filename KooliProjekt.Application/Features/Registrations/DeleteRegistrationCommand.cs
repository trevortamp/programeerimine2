using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Registrations
{
    public class DeleteRegistrationCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
