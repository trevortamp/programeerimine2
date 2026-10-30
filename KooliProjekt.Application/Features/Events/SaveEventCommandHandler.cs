using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Events
{
    public class SaveEventCommandHandler : IRequestHandler<SaveEventCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveEventCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveEventCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            Event entity;
            if (request.Id == 0)
            {
                entity = new Event();
                _dbContext.Events.Add(entity);
            }
            else
            {
                entity = await _dbContext.Events.FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return result.AddError("Event with ID " + request.Id + " was not found");
                }
            }

            entity.Name = request.Name;
            entity.StartDateTime = request.StartDateTime;
            entity.EndDateTime = request.EndDateTime;
            entity.Description = request.Description;
            entity.Price = request.Price;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
