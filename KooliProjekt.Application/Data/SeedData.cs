using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using KooliProjekt.Application.Infrastructure.Security;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        private static readonly string[] FirstNames = { "Mari", "Jaan", "Liis", "Karl", "Kaarel", "Anna", "Peeter", "Katrin", "Andres", "Maria", "Toomas", "Helena", "Mart", "Laura", "Siim", "Kadri", "Rainer", "Triin", "Margus", "Eliise", "Taavi", "Piret", "Henri", "Annika", "Oliver", "Kristiina", "Martin", "Merike", "Indrek", "Ketlin", "Sander", "Johanna", "Hannes", "Birgit", "Raivo", "Anette", "Urmas", "Sirje", "Joonas", "Evelin" };
        private static readonly string[] LastNames = { "Tamm", "Saar", "Sepp", "Mägi", "Kask", "Kukk", "Rebane", "Ilves", "Pärn", "Koppel", "Lepik", "Oja", "Kallas", "Org", "Lill", "Luik", "Vaher", "Kivi", "Põld", "Mets", "Raud", "Jõgi", "Soo", "Laine", "Toom", "Ots", "Teder", "Rand", "Karu", "Pihlak", "Aas", "Sild", "Eenok", "Nurm", "Paju", "Tomson", "Viik", "Lember", "Arro", "Valk" };

        public static void Generate(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var users = SeedUsers(context);
            var events = SeedEvents(context);
            var registrations = SeedRegistrations(context, users, events);
            var invoices = SeedInvoices(context, registrations, events);
            SeedPayments(context, invoices);
        }

        private static List<User> SeedUsers(ApplicationDbContext context)
        {
            // Kõigil testkasutajatel on sama parool: Parool123!
            var passwordHash = PasswordHasher.Hash("Parool123!");
            var users = new List<User>();

            for (var i = 0; i < FirstNames.Length; i++)
            {
                var firstName = FirstNames[i];
                var lastName = LastNames[(i * 7 + 3) % LastNames.Length];
                var userName = RemoveDiacritics(firstName + "." + lastName).ToLowerInvariant();

                users.Add(new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = userName,
                    Email = userName + "@example.ee",
                    PasswordHash = passwordHash
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return users;
        }

        private static List<Event> SeedEvents(ApplicationDbContext context)
        {
            var data = new (string Name, string Description, decimal Price)[]
            {
                ("Tallinn Tech Summit", "Kahepäevane konverents tarkvaraarenduse, tehisintellekti ja startupide teemadel.", 149m),
                ("Tartu Linnamaraton", "Traditsiooniline jooksuüritus Tartu kesklinnas 10 ja 21 km distantsidel.", 35m),
                ("Jazzkaar Gala", "Rahvusvaheliste džässartistide kontsert Tallinna kontserdisaalis.", 45m),
                ("Eesti Toidufestival", "Kohalike tootjate degusteerimine ja kokandusmeistriklassid.", 15m),
                ("Digiturunduse töötuba", "Praktiline töötuba sotsiaalmeedia reklaami ja SEO alustest.", 89m),
                ("Startup Wisdom Night", "Ettevõtjate õhtu esinejate, võrgustumise ja pitch'idega.", 25m),
                ("Kevadine jalgrattamatk", "Perematk Pirita rajal koos peatuste ja suupistetega.", 10m),
                ("Loodusfotograafia õppepäev", "Päev Lahemaa rahvuspargis koos professionaalse fotograafiga.", 55m),
                ("Ooperiõhtu: Luikede järv", "Klassikalise balleti etendus rahvusooperi laval.", 60m),
                ("Kodeerimise noortelaager", "Nädalane laager 12-16-aastastele Pythoni ja mänguarenduse algõpetusega.", 120m),
                ("Veinidegusteerimise õhtu", "Kuue Euroopa veini degusteerimine sommeljee juhendamisel.", 40m),
                ("Karjääripäev", "Tööandjate mess üliõpilastele ja värsketele lõpetajatele.", 5m),
                ("Kirjandusõhtu vanalinnas", "Kohalike kirjanike ettelugemised ja vestlusring.", 12m),
                ("Kokandusmeistriklass: Eesti köök", "Traditsiooniliste roogade valmistamine kogenud kokaga.", 75m),
                ("Joogaretriit Saaremaal", "Kolmepäevane lõõgastusretriit mere ääres.", 210m),
                ("Ettevõtte jõulupidu", "Õhtusöök, elav muusika ja loosimäng.", 50m),
                ("Küberturvalisuse seminar", "Ülevaade levinumatest rünnetest ja kaitsemeetmetest ettevõtetele.", 95m),
                ("Power BI andmeanalüüsi koolitus", "Päevane praktiline koolitus aruannete ja dashboardide koostamiseks.", 130m),
                ("Noorte robootikavõistlus", "Koolinoorte meeskondade võistlus isesõitvate robotitega.", 8m),
                ("Linnaorienteerumine Tallinnas", "Meeskondlik mäng vanalinna tänavatel lahendatavate ülesannetega.", 20m),
                ("Kaasaegse kunsti näituse avamine", "Eesti ja Põhjamaade noorte kunstnike ühisnäituse avaõhtu.", 10m),
                ("Stand-up komöödiaõhtu", "Viie koomiku etteaste Tallinna klubis.", 22m),
                ("Projektijuhtimise sertifikaadikoolitus", "Kahepäevane koolitus agiilse projektijuhtimise alustest.", 290m),
                ("Fotograafia algkursus", "Kursus algajatele: avatus, säriaeg ja kompositsioon.", 70m),
                ("Rannavolle turniir Pärnus", "Amatööride meeskondlik rannavolle turniir.", 30m),
                ("Kammermuusika kontsert", "Keelpillikvarteti kontsert vanalinna kirikus.", 18m),
                ("Hackathon: Targa linna lahendused", "24 tunni programmeerimisvõistlus linnaruumi teemadel.", 0m),
                ("Lastelavastus: Kirjud lood", "Muusikaline etendus lastele alates 4. eluaastast.", 9m),
                ("Talvine metsamatk", "Gaidiga matk Soomaa rabas lõkkeõhtuga.", 28m),
                ("Jõuluturu avamine", "Käsitööliste müük, glögi ja kontsert Raekoja platsil.", 5m),
                ("Finantskirjaoskuse töötuba", "Isikliku eelarve, säästmise ja investeerimise alused.", 35m),
                ("UX/UI disaini meistriklass", "Praktiline töötuba kasutajaliideste prototüüpimisest.", 110m),
                ("Tervisliku toitumise loeng", "Toitumisnõustaja loeng ja küsimuste voor.", 12m),
                ("Podcastide tegemise töötuba", "Salvestamine, montaaž ja levitamine algajatele.", 65m),
                ("Aasta tarkvaraarendaja gala", "Tunnustusõhtu parimatele IT-spetsialistidele.", 85m),
                ("Mälumänguõhtu", "Meeskondlik mälumäng kolmes voorus.", 6m)
            };

            var events = new List<Event>();
            for (var i = 0; i < data.Length; i++)
            {
                var start = new DateTime(2026, 11, 2, 10, 0, 0).AddDays(i * 4).AddHours(i % 3 * 4);

                events.Add(new Event
                {
                    Name = data[i].Name,
                    Description = data[i].Description,
                    Price = data[i].Price,
                    StartDateTime = start,
                    EndDateTime = start.AddHours(2 + i % 4 * 2)
                });
            }

            context.Events.AddRange(events);
            context.SaveChanges();

            return events;
        }

        private static List<Registration> SeedRegistrations(ApplicationDbContext context, List<User> users, List<Event> events)
        {
            var registrations = new List<Registration>();

            // Iga kasutaja registreerub ühele üritusele (kasutaja + üritus on unikaalne paar)
            for (var i = 0; i < users.Count; i++)
            {
                registrations.Add(new Registration
                {
                    UserId = users[i].Id,
                    EventId = events[(i * 5 + 2) % events.Count].Id,
                    RegistrationDate = new DateTime(2026, 9, 1, 9, 0, 0).AddDays(i).AddMinutes(i * 17)
                });
            }

            context.Registrations.AddRange(registrations);
            context.SaveChanges();

            return registrations;
        }

        private static List<Invoice> SeedInvoices(ApplicationDbContext context, List<Registration> registrations, List<Event> events)
        {
            var invoices = new List<Invoice>();

            for (var i = 0; i < registrations.Count; i++)
            {
                var registration = registrations[i];
                var price = events.First(e => e.Id == registration.EventId).Price;

                invoices.Add(new Invoice
                {
                    RegistrationId = registration.Id,
                    InvoiceNumber = "ARVE-2026-" + (i + 1).ToString("D4"),
                    Amount = price
                });
            }

            context.Invoices.AddRange(invoices);
            context.SaveChanges();

            return invoices;
        }

        private static void SeedPayments(ApplicationDbContext context, List<Invoice> invoices)
        {
            var payments = new List<Payment>();

            for (var i = 0; i < invoices.Count; i++)
            {
                var status = PaymentStatus.Paid;
                if (i % 9 == 4) status = PaymentStatus.Pending;
                if (i % 13 == 7) status = PaymentStatus.Failed;
                if (i % 17 == 11) status = PaymentStatus.Refunded;

                payments.Add(new Payment
                {
                    InvoiceId = invoices[i].Id,
                    Amount = invoices[i].Amount,
                    PaymentDate = new DateTime(2026, 9, 3, 12, 0, 0).AddDays(i).AddMinutes(i * 23),
                    Status = status
                });
            }

            context.Payments.AddRange(payments);
            context.SaveChanges();
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
