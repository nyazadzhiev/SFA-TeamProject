using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL
{
    public class DatabaseSeeder
    {
        public static void Seed(DatabaseContext database)
        {
            if (database.Database.EnsureCreated())
            {
                database.Users.Add(new User()
                {
                    Email = "admin@admin.test",
                    Username = "admin@admin.test",
                    Password = "adminpass",
                    FirstName = "Admin",
                    LastName = "Admin",
                }); 

                database.SaveChanges();
            }
        }

    }
}
