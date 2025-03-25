using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Prog6_Assessment_CodyBoelens.Data.DbEntities
{
    [Table("Boeking")]
    public class Boeking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Is_Confirmed { get; set; }

        public ICollection<BeestjeBoeking> BeestBoekingen { get; set; }
        public int? KlantId { get; internal set; }
    }
}
