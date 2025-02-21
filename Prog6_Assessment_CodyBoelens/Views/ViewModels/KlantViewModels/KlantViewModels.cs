using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel
{

    public class KlantViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Gebruik een correct emailadres")]
        public string Email { get; set; }
        public string? Rank { get; set; }
        [RegularExpression(@"^06\d{8}$", ErrorMessage = "Gebruik juiste telefoonnummer format zoals: 0612345678")]
        public string? PhoneNumber { get; set; }
        public string Adres { get; set; }
        public int? KlantkaartId { get; set; }
        public List<Klantkaart>? allRanks { get; set; }
    }
}
