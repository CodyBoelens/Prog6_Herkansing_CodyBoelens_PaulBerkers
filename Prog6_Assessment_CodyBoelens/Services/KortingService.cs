using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Services
{
    public class KortingService : IKortingService
    {
        private static readonly Random _random = new Random();

        public double BerekenKorting(KlantViewModels klant, double totaalPrijs, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            double totaleKorting = 0.0;

            totaleKorting += KortingDrieDierenVanZelfdeType(totaalPrijs, selectedBeestjes);
            totaleKorting += KortingEendKans(totaalPrijs, selectedBeestjes);
            totaleKorting += KortingOpWeekdag(totaalPrijs, datum);
            totaleKorting += KortingVoorLettersInNaam(selectedBeestjes);
            totaleKorting += KortingVoorKlantenkaart(totaalPrijs, klant);

            // Maximaal 60% korting
            return Math.Min(totaleKorting, totaalPrijs * 0.60);
        }

        public double KortingDrieDierenVanZelfdeType(double totaalPrijs, List<BeestjeViewModels> selectedBeestjes)
        {
            if (selectedBeestjes.GroupBy(b => b.Type).Any(g => g.Count() >= 3))
            {
                return totaalPrijs * 0.10;
            }
            return 0.0;
        }

        public double KortingEendKans(double totaalPrijs, List<BeestjeViewModels> selectedBeestjes)
        {
            if (selectedBeestjes.Any(b => b.Name == "Eend") && _random.Next(1, 7) == 1)
            {
                return totaalPrijs * 0.50;
            }
            return 0.0;
        }

        public double KortingOpWeekdag(double totaalPrijs, DateTime datum)
        {
            if (datum.DayOfWeek == DayOfWeek.Monday || datum.DayOfWeek == DayOfWeek.Tuesday)
            {
                return totaalPrijs * 0.15;
            }
            return 0.0;
        }

        public double KortingVoorLettersInNaam(List<BeestjeViewModels> selectedBeestjes)
        {
            double korting = 0.0;

            foreach (var beestje in selectedBeestjes)
            {
                string naam = beestje.Name.ToUpper(); // Maak de naam hoofdletters om case-insensitive te vergelijken
                int letterCount = 0;

                // Definieer de opeenvolgende letters van A tot Z
                var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

                // Kijk voor opeenvolgende letters
                for (int i = 0; i < letters.Length; i++)
                {
                    char currentLetter = letters[i];
                    if (naam.Contains(currentLetter))
                    {
                        letterCount++;
                    }
                    else
                    {
                        break; // Stop met het verhogen van letterCount zodra een letter ontbreekt
                    }
                }

                // Bereken de korting: 2% per opeenvolgende letter
                korting += letterCount * 0.02; // 2% korting per letter
            }

            return korting;
        }


        public double KortingVoorKlantenkaart(double totaalPrijs, KlantViewModels klant)
        {
            // Controleer of de klant een klantenkaart heeft (bijvoorbeeld: KlantkaartId != 0)
            if (klant.KlantkaartId != 0)
            {
                return totaalPrijs * 0.10;  // 10% korting voor klanten met een klantenkaart
            }

            return 0.0;  // Geen korting als er geen klantenkaart is
        }

    }

}
