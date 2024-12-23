using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Hsm.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HsmServer.IntegrationTest
{
    public class HospitalTestDbContext : HsmDbContext
    {
        public HospitalTestDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //HospitalSeedData(modelBuilder, "../../../data/hospital.json");
            //AppUserSeedData(modelBuilder, "../../../data/user.json");
        }

        private async void AppUserSeedData(ModelBuilder modelBuilder, string file)
        {
            var passwordHasher = new PasswordHasher<AppUser>();

            AppUser appUser = new()
            {
                TcNumber = "12345678910",
                Email = "user@gmail.com",
                IsActive = true,
                Id = Guid.NewGuid(),
                UserName = "User",
                EmailConfirmed = true,
                PhoneNumber = "5556667788",
                CreatedDateUTC = DateTime.UtcNow
            };

            appUser.PasswordHash = passwordHasher.HashPassword(appUser, "User_123");

            modelBuilder.Entity<AppUser>().HasData(appUser);
        }

        private void HospitalSeedData(ModelBuilder modelBuilder, string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                var json = reader.ReadToEnd();

                Hospital[] data;
                try
                {
                    data = JsonConvert.DeserializeObject<Hospital[]>(json);
                }
                catch (JsonSerializationException)
                {
                    var singleObject = JsonConvert.DeserializeObject<Hospital>(json);
                    data = new[] { singleObject };
                }

                modelBuilder.Entity<Hospital>().HasData(data);
            }
        }
    }
}
