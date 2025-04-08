using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Prog6_Assessment_CodyBoelens.Data.DbEntities
{
    [ExcludeFromCodeCoverage]
    [Table("Types")]
    public class Types
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }
    }
}
