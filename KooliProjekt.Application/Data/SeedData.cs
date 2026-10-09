using System;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        // Basic seed method - add your data here
        public static void Generate(ApplicationDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Example: only run when database has no users
            if (!context.Users.Any())
            {
                // Add your seeding logic here. Example skeleton:
                // var users = new List<User>();
                // for (int i = 0; i < 35; i++) { users.Add(new User { UserName = $"user{i}", ... }); }
                // context.Users.AddRange(users);
                // context.SaveChanges();
            }
        }



    }

}
