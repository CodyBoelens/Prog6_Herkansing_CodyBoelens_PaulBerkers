using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Prog6_Assessment_CodyBoelens.Data.DbEntities
{
    [ExcludeFromCodeCoverage]
    [Table("Beestje")]
    public class Beestje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [StringLength(50)]
        public string Picture { get; set; }
        [Required]
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public Types Type { get; set; }

        public ICollection<BeestjeBoeking> BeestBoekingen { get; set; }
    }
}
