using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel
{
    [ExcludeFromCodeCoverage]

    public class KlantViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Gebruik een correct emailadres")]
        public string Email { get; set; }

        public string? Rank { get; set; }

        [Required(ErrorMessage = "Telefoonnummer is verplicht")]
        [RegularExpression(@"^06\d{8}$", ErrorMessage = "Gebruik juiste telefoonnummer format zoals: 0612345678")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Adres is verplicht")]
        public string Adres { get; set; }
        public int? KlantkaartId { get; set; }
        public List<Klantkaart>? allRanks { get; set; }
    }
}
