using System.ComponentModel.DataAnnotations.Schema;

namespace Prog6_Assessment_CodyBoelens.Data.DbEntities
{

    [Table("BeestjeBoeking")]
    public class BeestjeBoeking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Beestje")]
        public int BeestjeID { get; set; }
        [ForeignKey("Boeking")]
        public int BoekingID { get; set; }
    }
}
