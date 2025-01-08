using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using System.ComponentModel.DataAnnotations;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel
{
    public class BeestjeViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Type { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Picture { get; set; }
        [Required]
        public int TypeId { get; set; }
        public List<Types>? allTypes { get; set; }
    }
}
