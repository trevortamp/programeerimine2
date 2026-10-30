using System;
using System.Collections.Generic;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public string UserFullName { get; set; }
        public string EventName { get; set; }
        public decimal PaidAmount { get; set; }
        public IList<PaymentSummaryDto> Payments { get; set; }

        public InvoiceDto()
        {
            Payments = new List<PaymentSummaryDto>();
        }
    }
}
