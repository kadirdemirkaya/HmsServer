using EfCore.Repository.Abstractions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hsm.Persistence.Context
{
    public static class HsmSeedDb
    {
        private static int count = 0;
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (count == 0)
            {
                var dbContext = serviceProvider.GetRequiredService<HsmDbContext>();

                await dbContext.Database.MigrateAsync();

                await UserAndRole(serviceProvider);

                await DataForFilter(serviceProvider);

                count += 1;
            }
        }

        private static async Task DataForFilter(IServiceProvider serviceProvider)
        {
            IReadRepository<Clinical> _readRepo = serviceProvider.GetRequiredService<IReadRepository<Clinical>>();
            IWriteRepository<Clinical> _writeClinicalRepo = serviceProvider.GetRequiredService<IWriteRepository<Clinical>>();
            IWriteRepository<Hospital> _writeHospitalRepo = serviceProvider.GetRequiredService<IWriteRepository<Hospital>>();
            IWriteRepository<Doctor> _writeDoctorRepo = serviceProvider.GetRequiredService<IWriteRepository<Doctor>>();
            UserManager<AppUser> _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            int clinicalCount = await _readRepo.CountAsync();

            if (clinicalCount == 0)
            {
                var defaultUsers = new List<AppUser>
                {
                    new()
                    {
                        Id = Guid.Parse("F27BBB36-6796-4496-B990-EBA9C1E02D74"),
                        UserName = "AhmetYılmaz",
                        Email = "AhmetYılmaz@example.com",
                        EmailConfirmed = true,
                        TcNumber = "12345678900"
                    },
                    new()
                    {
                        Id = Guid.Parse("B7D8A1DD-2B3A-4AA4-8DDC-18D3E94E8F40"),
                        UserName = "MerveKaya",
                        Email = "MerveKaya@example.com",
                        EmailConfirmed = true,
                        TcNumber = "12345678999"
                    },
                    new()
                    {
                        Id = Guid.Parse("9C25EED7-8B5F-4169-AA71-8EC3DEB9376D"),
                        UserName = "Zeynep",
                        Email = "Zeynep@example.com",
                        EmailConfirmed = true,
                        TcNumber = "12345623454"
                    },
                    new()
                    {
                        Id = Guid.Parse("0BDB1DF6-66FB-4444-A95B-632021AB80E3"),
                        UserName = "EmreÇelik",
                        Email = "EmreÇelik@example.com",
                        EmailConfirmed = true,
                        TcNumber = "01010192925"
                    },
                    new()
                    {
                        Id = Guid.Parse("2042D625-7A37-4E11-8F8E-81E1FFC480ED"),
                        UserName = "HakanDemir",
                        Email = "HakanDemir@example.com",
                        EmailConfirmed = true,
                        TcNumber = "2323121223345"
                    },
                    new()
                    {
                        Id = Guid.Parse("28B86295-4497-4F39-9BBD-FA5B2713E2E9"),
                        UserName = "LeylaŞahin",
                        Email = "LeylaŞahin@example.com",
                        EmailConfirmed = true,
                        TcNumber = "12343215674"
                    },
                    new()
                    {
                        Id = Guid.Parse("28B86295-4497-4F39-9BBD-FA5B2713E2E9"),
                        UserName = "HakanDemir",
                        Email = "HakanDemir@example.com",
                        EmailConfirmed = true,
                        TcNumber = "12323112332"
                    },
                     new()
                    {
                        Id = Guid.Parse("749499D5-281E-4C12-BF05-845BD0628605"),
                        UserName = "CanÇelik",
                        Email = "CanÇelik@example.com",
                        EmailConfirmed = true,
                        TcNumber = "234432234432"
                    },
                      new()
                    {
                        Id = Guid.Parse("CC9F9403-C87C-4989-A70E-1702C9300500"),
                        UserName = "AyşeKoç",
                        Email = "AyşeKoç@example.com",
                        EmailConfirmed = true,
                        TcNumber = "56776556776"
                    }
                };
                defaultUsers.Add(new()
                {
                    Id = Guid.Parse("C04E99E0-D639-48CC-9B21-4B0105342370"),
                    UserName = "MehmetYılmaz",
                    Email = "MehmetYılmaz@example.com",
                    EmailConfirmed = true,
                    TcNumber = "12340008900"
                });
                defaultUsers.Add(new()
                {
                    Id = Guid.Parse("ECFF321E-FDDB-4C7C-A7D1-4BE0985FECC9"),
                    UserName = "SelinArslan",
                    Email = "SelinArslan@example.com",
                    EmailConfirmed = true,
                    TcNumber = "11140008900"
                });
                defaultUsers.Add(new()
                {
                    Id = Guid.Parse("4CE15744-9ECE-4D79-856B-E0CE9BC7C797"),
                    UserName = "SelinArslan",
                    Email = "SelinArslan@example.com",
                    EmailConfirmed = true,
                    TcNumber = "11140008900"
                });
                defaultUsers.Add(new()
                {
                    Id = Guid.Parse("8A50CD69-71C1-4DFB-AA0B-B08EBA654C95"),
                    UserName = "AliDemir",
                    Email = "AliDemir@example.com",
                    EmailConfirmed = true,
                    TcNumber = "11140008922"
                });
                defaultUsers.Add(new()
                {
                    Id = Guid.Parse("05B126AF-4B08-4930-B244-2E2121318273"),
                    UserName = "AyhanKara",
                    Email = "AyhanKara@example.com",
                    EmailConfirmed = true,
                    TcNumber = "13340008922"
                });
                defaultUsers.Add(new()
                {
                    Id = Guid.Parse("F1A8E205-C476-495C-ABB3-B4F0E175E833"),
                    UserName = "ZeynepÇelik",
                    Email = "ZeynepÇelik@example.com",
                    EmailConfirmed = true,
                    TcNumber = "14440008922"
                });

                foreach (var defaultUser in defaultUsers)
                {
                    var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        var result = await _userManager.CreateAsync(defaultUser, "user_123");
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(defaultUser, "Admin");
                        }
                    }
                }

                #region 1. Hastane
                Hospital hospital = Hospital.Create("Ankara Şehir Hastanesi", Address.Create("Bilkent Caddesi", "İstanbul", "active", "06800", "Türkiye", "Çankaya"), "+90 312 552 6000", City.Create("İstanbul"));

                Clinical clinical = Clinical.Create("Kardiyoloji", hospital.Id);
                hospital.AddClinicalToHospital(clinical);

                Doctor doctor1 = Doctor.Create("Dr. Ahmet", "Yılmaz", "Kardiyoloji Uzmanı", "Schedule", Guid.Parse("F27BBB36-6796-4496-B990-EBA9C1E02D74"));
                hospital.AddDoctorToHospital(doctor1);
                Doctor doctor2 = Doctor.Create("Dr. Merve", "Kaya", "Kardiyoloji Uzmanı", "Schedule", Guid.Parse("B7D8A1DD-2B3A-4AA4-8DDC-18D3E94E8F40"));
                hospital.AddDoctorToHospital(doctor2);
                #endregion

                #region 2. Hastane
                Hospital hospital2 = Hospital.Create(
                    "İstanbul Eğitim ve Araştırma Hastanesi",
                    Address.Create("Fatih Caddesi", "İstanbul", "active", "34000", "Türkiye", "Fatih"),
                    "+90 212 453 1700",
                    City.Create("İstanbul")
                );

                Clinical orthopedics2 = Clinical.Create("Ortopedi", hospital2.Id);
                hospital2.AddClinicalToHospital(orthopedics2);

                Doctor doctor2_1 = Doctor.Create("Dr. Zeynep", "Öztürk", "Ortopedi Uzmanı", "Schedule", Guid.Parse("9C25EED7-8B5F-4169-AA71-8EC3DEB9376D"));
                Doctor doctor2_2 = Doctor.Create("Dr. Emre", "Çelik", "Ortopedi Uzmanı", "Schedule", Guid.Parse("0BDB1DF6-66FB-4444-A95B-632021AB80E3"));

                hospital2.AddDoctorToHospital(doctor2_1);
                hospital2.AddDoctorToHospital(doctor2_2);
                #endregion

                #region 3. Hastane
                Hospital hospital3 = Hospital.Create(
                    "İzmir Atatürk Eğitim ve Araştırma Hastanesi",
                    Address.Create("Kahramanlar Mahallesi", "İzmir", "active", "35040", "Türkiye", "Konak"),
                    "+90 232 243 4343",
                    City.Create("İzmir")
                );

                Clinical internalMedicine3 = Clinical.Create("Dahiliye", hospital3.Id);
                hospital3.AddClinicalToHospital(internalMedicine3);

                Doctor doctor3_1 = Doctor.Create("Dr. Hakan", "Demir", "Dahiliye Uzmanı", "Schedule", Guid.Parse("2042D625-7A37-4E11-8F8E-81E1FFC480ED"));
                Doctor doctor3_2 = Doctor.Create("Dr. Leyla", "Şahin", "Dahiliye Uzmanı", "Schedule", Guid.Parse("28B86295-4497-4F39-9BBD-FA5B2713E2E9"));

                hospital3.AddDoctorToHospital(doctor3_1);
                hospital3.AddDoctorToHospital(doctor3_2);
                #endregion

                #region 4. Hastane
                Hospital hospital4 = Hospital.Create(
                    "Bursa Şehir Hastanesi",
                    Address.Create("Nilüfer Caddesi", "Bursa", "active", "16110", "Türkiye", "Nilüfer"),
                    "+90 224 295 5000",
                    City.Create("Bursa")
                );

                Clinical pediatrics4 = Clinical.Create("Pediatri", hospital4.Id);
                hospital4.AddClinicalToHospital(pediatrics4);

                Doctor doctor4_1 = Doctor.Create("Dr. Can", "Çelik", "Pediatri Uzmanı", "Schedule", Guid.Parse("749499D5-281E-4C12-BF05-845BD0628605"));
                Doctor doctor4_2 = Doctor.Create("Dr. Ayşe", "Koç", "Pediatri Uzmanı", "Schedule", Guid.Parse("CC9F9403-C87C-4989-A70E-1702C9300500"));

                hospital4.AddDoctorToHospital(doctor4_1);
                hospital4.AddDoctorToHospital(doctor4_2);
                #endregion

                #region 5. Hastane
                Hospital hospital5 = Hospital.Create(
                    "İstanbul Medipol Mega Hastanesi",
                    Address.Create("Bağcılar Caddesi", "İstanbul", "active", "34200", "Türkiye", "Bağcılar"),
                    "+90 212 460 7777",
                    City.Create("İstanbul")
                );

                Clinical cardiology5 = Clinical.Create("Kardiyoloji", hospital5.Id);
                hospital5.AddClinicalToHospital(cardiology5);

                Doctor doctor5_1 = Doctor.Create("Dr. Mehmet", "Yılmaz", "Kardiyolog", "Schedule", Guid.Parse("C04E99E0-D639-48CC-9B21-4B0105342370"));
                Doctor doctor5_2 = Doctor.Create("Dr. Selin", "Arslan", "Kardiyolog", "Schedule", Guid.Parse("ECFF321E-FDDB-4C7C-A7D1-4BE0985FECC9"));
                Doctor doctor5_3 = Doctor.Create("Dr. Ali", "Demir", "Kardiyolog", "Schedule", Guid.Parse("8A50CD69-71C1-4DFB-AA0B-B08EBA654C95"));

                hospital5.AddDoctorToHospital(doctor5_1);
                hospital5.AddDoctorToHospital(doctor5_2);
                hospital5.AddDoctorToHospital(doctor5_3);
                #endregion

                #region 6. Hastane 
                Hospital hospital6 = Hospital.Create(
                    "Ankara Şehir Hastanesi",
                    Address.Create("Kızılırmak Mahallesi", "Ankara", "active", "06800", "Türkiye", "Çankaya"),
                    "+90 312 552 6000",
                    City.Create("Ankara")
                );

                Clinical oncology6 = Clinical.Create("Onkoloji", hospital6.Id);
                hospital6.AddClinicalToHospital(oncology6);

                Doctor doctor6_1 = Doctor.Create("Dr. Ayhan", "Kara", "Onkolog", "Schedule", Guid.Parse("05B126AF-4B08-4930-B244-2E2121318273"));
                Doctor doctor6_2 = Doctor.Create("Dr. Zeynep", "Çelik", "Onkolog", "Schedule", Guid.Parse("F1A8E205-C476-495C-ABB3-B4F0E175E833"));

                hospital6.AddDoctorToHospital(doctor6_1);
                hospital6.AddDoctorToHospital(doctor6_2);
                #endregion

                await _writeHospitalRepo.AddAsync(hospital);
                await _writeHospitalRepo.AddAsync(hospital2);
                await _writeHospitalRepo.AddAsync(hospital3);
                await _writeHospitalRepo.AddAsync(hospital4);
                await _writeHospitalRepo.AddAsync(hospital5);
                await _writeHospitalRepo.AddAsync(hospital6);
                await _writeDoctorRepo.SaveChangesAsync();
            }
        }

        private static async Task UserAndRole(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            if (userManager.Users.Count() == 0 || roleManager.Roles.Count() == 0)
            {
                var roles = new[] { "Admin", "Moderator", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new AppRole { Name = role, NormalizedName = role.ToUpper() });
                    }
                }

                var defaultUsers = new List<AppUser>
                {
                    new()
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                        TcNumber = "2223335566"
                    },
                    new()
                    {
                        UserName = "user@example.com",
                        Email = "user@example.com",
                        EmailConfirmed = true,
                        TcNumber = "1123435577"
                    }
                };

                foreach (var defaultUser in defaultUsers)
                {
                    var user = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        var result = await userManager.CreateAsync(defaultUser, "Admin123!");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(defaultUser, "Admin");
                        }
                    }
                }
            }
        }
    }

}
