using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Prog6_Assessment_CodyBoelens.Data.DbEntities
{
    [ExcludeFromCodeCoverage]
    [Table("Klant")]
    public class Klant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adres { get; set; }
        public int KlantkaartId { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
    }
}
