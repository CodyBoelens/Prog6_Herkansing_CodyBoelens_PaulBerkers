using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using System.ComponentModel.DataAnnotations;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel
{
    public class BeestjeViewModels
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Type { get; set; }

        [Required]
        [Range(0.01, 10000, ErrorMessage = "Prijs moet tussen 0.01 en 999.99 liggen.")]
        public double Price { get; set; }
        [Required]
        public string Picture { get; set; }

        [Required(ErrorMessage = "Type is verplicht")]
        public int TypeId { get; set; }
        public List<Types>? allTypes { get; set; }
        public List<string>? allImageNames { get; set; }
    }
}
