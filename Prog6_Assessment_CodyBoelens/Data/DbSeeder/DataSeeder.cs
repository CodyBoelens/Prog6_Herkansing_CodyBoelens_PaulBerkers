using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;

namespace Prog6_Assessment_CodyBoelens.Data.DbSeeder
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedData()
        {
            SeedRolesAsync().Wait(); // Wait for the asynchronous seeding to complete
            SeedUserAsync().Wait();
            SeedTypes();
            SeedBeestjes();
            SeedKlantkaarten();

        }

        private void SeedBeestjes()
        {
            if (!_context.Beestjes.Any())
            {
                var beestjes = new List<Beestje>
                {
                    new Beestje()
                    {
                        Name = "Aap",
                        Price = 300,
                        Picture = "aap.jpeg",
                        TypeId = 1,
                    },
                    new Beestje()
                    {
                        Name = "Leeuw",
                        Price = 750,
                        Picture = "leeuw.jpg",
                        TypeId = 1,
                    },
                    new Beestje()
                    {
                        Name = "Zebra",
                        Price = 450,
                        Picture = "zebra.jpeg",
                        TypeId = 1,
                    },
                    new Beestje()
                    {
                        Name = "Hond",
                        Price = 50,
                        Picture = "hond.jpg",
                        TypeId = 2,
                    },
                    new Beestje()
                    {
                        Name = "Kuiken",
                        Price = 30,
                        Picture = "kuiken.jpeg",
                        TypeId = 2,
                    },
                    new Beestje()
                    {
                        Name = "Ezel",
                        Price = 95,
                        Picture = "ezel.jpeg",
                        TypeId = 2,
                    },
                    new Beestje()
                    {
                        Name = "Koe",
                        Price = 150,
                        Picture = "koe.jpg",
                        TypeId = 2,
                    },
                    new Beestje()
                    {
                        Name = "Pinguin",
                        Price = 350,
                        Picture = "pinguin.jpeg",
                        TypeId = 3,
                    },
                    new Beestje()
                    {
                        Name = "Ijsbeer",
                        Price = 900,
                        Picture = "ijsbeer.jpeg",
                        TypeId = 3,
                    },
                    new Beestje()
                    {
                        Name = "Zeehond",
                        Price = 400,
                        Picture = "zeehond.jpeg",
                        TypeId = 3,
                    },
                    new Beestje()
                    {
                        Name = "Kameel",
                        Price = 200,
                        Picture = "kameel.jpg",
                        TypeId = 4,
                    },
                    new Beestje()
                    {
                        Name = "Slang",
                        Price = 80,
                        Picture = "slang.jpeg",
                        TypeId = 4,
                    },
                    new Beestje()
                    {
                        Name = "T-Rex",
                        Price = 1800,
                        Picture = "T-Rex.jpg",
                        TypeId = 5,
                    },
                    new Beestje()
                    {
                        Name = "Unicorn",
                        Price = 1200,
                        Picture = "unicorn.jpeg",
                        TypeId = 5,
                    }
                };

                _context.Beestjes.AddRange(beestjes);
                _context.SaveChanges();
            }
        }

        private void SeedTypes()
        {
            if (!_context.Types.Any())
            {
                var types = new List<Types>
                {
                    new Types()
                    {
                        TypeName = "Jungle"
                    },
                    new Types()
                    {
                        TypeName = "Boerderij"
                    },
                    new Types()
                    {
                        TypeName = "Sneeuw"
                    },
                    new Types()
                    {
                        TypeName = "Woestijn"
                    },
                    new Types()
                    {
                        TypeName = "VIP"
                    }
                };

                _context.Types.AddRange(types);
                _context.SaveChanges();
            }
        }

        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                // Seed roles
                var boerderijRole = new IdentityRole
                {
                    Name = "Boerderij"
                };

                var klantRole = new IdentityRole
                {
                    Name = "Klant"
                };

                await _roleManager.CreateAsync(boerderijRole);
                await _roleManager.CreateAsync(klantRole);
            }
        }

        private async Task SeedUserAsync()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user = new IdentityUser
                {
                    UserName = "manager@email.com",
                    Email = "manager@email.com",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "@Test1");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Boerderij");
                }
                else
                {
                    var errors = result.Errors;
                    // Handle errors appropriately
                }
            }
        }

        public void SeedKlantkaarten()
        {
            if (!_context.Klantkaarten.Any())
            {
                var klantkaarten = new List<Klantkaart>
                {
                    new Klantkaart()
                    {
                        Rank = "Zilver"
                    },
                    new Klantkaart()
                    {
                        Rank = "Goud"
                    },
                    new Klantkaart()
                    {
                        Rank = "Platina"
                    }
                };

                _context.Klantkaarten.AddRange(klantkaarten);
                _context.SaveChanges();
            }
        }
    }
}
