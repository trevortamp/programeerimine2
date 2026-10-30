using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class SavePaymentCommandHandler : IRequestHandler<SavePaymentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePaymentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePaymentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var invoiceExists = await _dbContext.Invoices.AnyAsync(x => x.Id == request.InvoiceId, cancellationToken);
            if (!invoiceExists)
            {
                return result.AddPropertyError("InvoiceId", "Cannot find invoice with ID " + request.InvoiceId);
            }

            Payment entity;
            if (request.Id == 0)
            {
                entity = new Payment();
                _dbContext.Payments.Add(entity);
            }
            else
            {
                entity = await _dbContext.Payments.FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return result.AddError("Payment with ID " + request.Id + " was not found");
                }
            }

            entity.InvoiceId = request.InvoiceId;
            entity.Amount = request.Amount;
            entity.PaymentDate = request.PaymentDate == default ? DateTime.Now : request.PaymentDate;
            entity.Status = request.Status;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
