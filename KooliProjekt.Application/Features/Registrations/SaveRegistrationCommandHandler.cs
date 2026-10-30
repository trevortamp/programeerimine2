using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Registrations
{
    public class SaveRegistrationCommandHandler : IRequestHandler<SaveRegistrationCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveRegistrationCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveRegistrationCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var userExists = await _dbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
            if (!userExists)
            {
                return result.AddPropertyError("UserId", "Cannot find user with ID " + request.UserId);
            }

            var eventExists = await _dbContext.Events.AnyAsync(x => x.Id == request.EventId, cancellationToken);
            if (!eventExists)
            {
                return result.AddPropertyError("EventId", "Cannot find event with ID " + request.EventId);
            }

            var alreadyRegistered = await _dbContext.Registrations
                .AnyAsync(x => x.UserId == request.UserId && x.EventId == request.EventId && x.Id != request.Id, cancellationToken);
            if (alreadyRegistered)
            {
                return result.AddError("This user is already registered for this event");
            }

            Registration entity;
            if (request.Id == 0)
            {
                entity = new Registration();
                _dbContext.Registrations.Add(entity);
            }
            else
            {
                entity = await _dbContext.Registrations.FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return result.AddError("Registration with ID " + request.Id + " was not found");
                }
            }

            entity.UserId = request.UserId;
            entity.EventId = request.EventId;
            entity.RegistrationDate = request.RegistrationDate == default ? DateTime.Now : request.RegistrationDate;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
