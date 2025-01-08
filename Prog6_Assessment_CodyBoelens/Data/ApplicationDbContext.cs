using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;

namespace Prog6_Assessment_CodyBoelens.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Beestje> Beestjes { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Klantkaart> Klantkaarten { get; set; }
    }
}