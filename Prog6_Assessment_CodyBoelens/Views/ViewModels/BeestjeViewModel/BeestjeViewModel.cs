using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using System.ComponentModel.DataAnnotations;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel
{
    public class BeestjeViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [StringLength(30, ErrorMessage = "De naam mag niet langer zijn dan 30 karakters.")]
        public string Name { get; set; }

        public string? Type { get; set; }

        [Required(ErrorMessage = "Prijs is verplicht")]
        [DataType(DataType.Currency)]
        [Range(0.01, 250000, ErrorMessage = "Prijs moet tussen 0.01 en 250000 liggen.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Plaatje is verplicht")]
        public string Picture { get; set; }

        [Required(ErrorMessage = "Type is verplicht")]
        [Range(1, int.MaxValue, ErrorMessage = "Type is verplicht")]
        public int TypeId { get; set; }

        public List<Types>? allTypes { get; set; }

        public List<string>? allImageNames { get; set; }
    }
}
