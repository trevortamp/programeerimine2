using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Registration)
                .WithOne(r => r.Invoice)
                .HasForeignKey<Invoice>(i => i.RegistrationId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public string Agenda { get; set; }
        public string Summary { get; set; }

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }

    public class Registration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool Paid { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
        public Invoice Invoice { get; set; }
    }

    public class Invoice
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime IssuedAt { get; set; }

        public Registration Registration { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }

        public Invoice Invoice { get; set; }
    }

}

