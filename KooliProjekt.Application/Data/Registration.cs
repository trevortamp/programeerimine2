using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(UserId), nameof(EventId), IsUnique = true)]
    public class Registration
    {
        public int Id { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public Event Event { get; set; }
        public int EventId { get; set; }

        public DateTime RegistrationDate { get; set; }

        public IList<Invoice> Invoices { get; set; }

        public Registration()
        {
            Invoices = new List<Invoice>();
        }
    }
}
