using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(InvoiceNumber), IsUnique = true)]
    public class Invoice
    {
        public int Id { get; set; }

        public Registration Registration { get; set; }
        public int RegistrationId { get; set; }

        [Required]
        [StringLength(30)]
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public IList<Payment> Payments { get; set; }

        public Invoice()
        {
            Payments = new List<Payment>();
        }
    }
}
