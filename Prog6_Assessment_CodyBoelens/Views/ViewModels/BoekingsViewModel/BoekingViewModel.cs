namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel
{
    public class BoekingViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Is_Confirmed { get; set; }
        public double TotaalPrijs { get; set; }
        public int? KlantId { get; set; }
        public List<int> BeestjeIds { get; set; } = new List<int>();
    }
}
