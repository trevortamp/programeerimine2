using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var registrationExists = await _dbContext.Registrations
                .AnyAsync(x => x.Id == request.RegistrationId, cancellationToken);
            if (!registrationExists)
            {
                return result.AddPropertyError("RegistrationId", "Cannot find registration with ID " + request.RegistrationId);
            }

            var numberTaken = await _dbContext.Invoices
                .AnyAsync(x => x.InvoiceNumber == request.InvoiceNumber && x.Id != request.Id, cancellationToken);
            if (numberTaken)
            {
                return result.AddPropertyError("InvoiceNumber", "Invoice number '" + request.InvoiceNumber + "' already exists");
            }

            Invoice entity;
            if (request.Id == 0)
            {
                entity = new Invoice();
                _dbContext.Invoices.Add(entity);
            }
            else
            {
                entity = await _dbContext.Invoices.FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return result.AddError("Invoice with ID " + request.Id + " was not found");
                }
            }

            entity.RegistrationId = request.RegistrationId;
            entity.InvoiceNumber = request.InvoiceNumber;
            entity.Amount = request.Amount;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
