using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prog6_Assessment_CodyBoelens.Data.DbEntities
{
    [Table("Klantkaart")]
    public class Klantkaart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Rank { get; set; }
    }
}
