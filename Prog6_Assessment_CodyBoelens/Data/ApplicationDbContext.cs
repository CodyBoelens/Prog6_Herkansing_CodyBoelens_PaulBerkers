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

        public virtual DbSet<Beestje> Beestjes { get; set; }
        public virtual DbSet<Types> Types { get; set; }
        public virtual DbSet<Klant> Klanten { get; set; }
        public virtual DbSet<Klantkaart> Klantkaarten { get; set; }
        public virtual DbSet<Boeking> Boekingen { get; set; }
        public virtual DbSet<BeestjeBoeking> BeestjeBoekingen { get; set; }
    }
}