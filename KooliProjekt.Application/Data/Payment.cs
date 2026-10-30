using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Payment
    {
        public int Id { get; set; }

        public Invoice Invoice { get; set; }
        public int InvoiceId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
